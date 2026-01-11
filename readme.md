# 机械手控制指令与工作机制说明文档

## 一、通信基础配置

### 1.1 网络连接参数
- **服务器IP**: `192.168.4.1`
- **左手端口**: `3001` (select_hand = false)
- **右手端口**: `4001` (select_hand = true)
- **Socket读取超时**: `600ms`
- **命令超时重试**: `800ms`
- **最大重试次数**: `1次`

### 1.2 通信协议格式
所有指令遵循固定格式：
- **起始标识**: `0xAA 0xBB`
- **结束标识**: `0xCC`
- **数据部分**: 根据指令类型变化

---

## 二、控制指令清单

### 2.1 主界面指令 (MainActivity)

#### 进入主界面指令
```
指令格式: AA BB FF CC
字节数组: [0xAA, 0xBB, 0xFF, 0xCC]
发送时机: MainActivity.onCreate() 时同时发送给左右手
重试机制: 800ms超时，最多重试1次
响应处理: 仅记录日志，不验证状态
```

**工作机制**:
- 同时建立左右手两个网络连接
- 分别发送进入指令到两个端口
- 左右手使用独立的Handler和重试机制
- ⚠️ **注意**: retryCount是共享变量，可能导致左右手重试互相影响

---

### 2.2 自动控制模式指令 (AutoControlActivity)

#### 进入自动控制模式
```
指令格式: AA BB 01 CC
字节数组: [0xAA, 0xBB, 0x01, 0xCC]
发送时机: AutoControlActivity.onCreate()
重试机制: 800ms超时，最多重试1次
状态验证: 收到 "state auto\r\n" 后停止重试
```

#### 退出自动控制模式
```
指令格式: AA BB FF CC
字节数组: [0xAA, 0xBB, 0xFF, 0xCC]
发送时机: AutoControlActivity.onDestroy()
重试机制: 无（直接发送，不等待响应）
```

#### 执行自动程序指令
```
指令格式: AA BB [程序号] FF CC
字节数组: [0xAA, 0xBB, programType, 0xFF, 0xCC]
程序号范围: 0x01 ~ 0x04
  - 0x01: image_auto_1 (程序1)
  - 0x02: image_auto_2 (程序2)
  - 0x03: image_auto_3 (程序3)
  - 0x04: image_auto_4 (程序4)
发送时机: 用户点击对应程序按钮时
重试机制: 无（直接发送）
```

**示例**:
- 程序1: `AA BB 01 FF CC`
- 程序2: `AA BB 02 FF CC`
- 程序3: `AA BB 03 FF CC`
- 程序4: `AA BB 04 FF CC`

---

### 2.3 手动控制模式指令 (HandControlActivity)

#### 进入手动控制模式
```
指令格式: AA BB 02 CC
字节数组: [0xAA, 0xBB, 0x02, 0xCC]
发送时机: HandControlActivity.onCreate()
重试机制: 800ms超时，最多重试1次
状态验证: 收到 "state manual\r\n" 后停止重试
```

#### 退出手动控制模式
```
指令格式: AA BB FF CC
字节数组: [0xAA, 0xBB, 0xFF, 0xCC]
发送时机: HandControlActivity.onDestroy()
重试机制: 无（直接发送，不等待响应）
```

#### 电机控制指令
```
指令格式: AA BB [电机序号] [档位值] CC
字节数组: [0xAA, 0xBB, motorIndex, settingValue, 0xCC]
数据长度: 5字节

电机序号映射:
  0x01: 拇指 (hand_slider_thumb)
  0x02: 食指 (hand_slider_index)
  0x03: 中指 (hand_slider_middle)
  0x04: 无名指 (hand_slider_ring)
  0x05: 小指 (hand_slider_pinky)
  0x06: 手肘 (hand_slider_elbow)

档位值范围:
  - 手指 (0x01~0x05): 1~30 (0x01~0x1E)
  - 手肘 (0x06): 1~20 (0x01~0x14)

发送时机: 用户松开滑块时 (ACTION_UP)
触发条件: 滑块值发生变化
重试机制: 无（直接发送）
```

**示例**:
- 拇指档位15: `AA BB 01 0F CC`
- 食指档位20: `AA BB 02 14 CC`
- 手肘档位10: `AA BB 06 0A CC`

**工作机制**:
- 使用OnTouchListener监听滑块
- 仅在ACTION_UP时发送（避免频繁发送）
- 比较当前值与上次值，只有变化时才发送
- 自动限制档位值在有效范围内

---


## 三、工作机制详解

### 3.1 网络通信机制 (NetworkUtils)

#### 连接建立流程
```
1. 创建NetworkUtils实例
   ↓
2. 根据select参数选择端口 (false=3001, true=4001)
   ↓
3. 异步建立TCP Socket连接 (ConnectTask)
   ↓
4. 获取InputStream和OutputStream
   ↓
5. 连接失败时调用onFailure回调
```

#### 数据发送流程
```
1. 调用sendData(byte[] data)
   ↓
2. 创建SendDataTask异步任务
   ↓
3. 检查Socket连接状态
   ↓
4. 设置Socket读取超时600ms
   ↓
5. 发送数据到OutputStream
   ↓
6. 立即读取响应数据 (最多等待600ms)
   ↓
7. 成功: 调用onSuccess(byte[] response)
   失败: 调用onFailure(String errorMessage)
```

#### 超时处理
- **Socket读取超时**: 600ms（在SendDataTask中设置）
- **命令超时重试**: 800ms（在Activity的Handler中设置）
- **超时触发**: 如果800ms内未收到成功响应，触发重试机制

---

### 3.2 超时重试机制

#### 重试流程
```
发送指令
   ↓
启动800ms定时器 (Handler.postDelayed)
   ↓
等待响应
   ├─ 成功收到响应 → 取消定时器，停止重试
   └─ 800ms超时 → 检查重试次数
       ├─ retryCount < MAX_RETRIES (1) → 重试发送
       └─ retryCount >= MAX_RETRIES → 放弃，记录日志
```

#### 状态验证机制
不同Activity验证不同的状态字符串:
- **AutoControlActivity**: 验证 `"state auto\r\n"`
- **HandControlActivity**: 验证 `"state manual\r\n"`
- **SettingActivity**: 验证 `"state set\r\n"`
- **MainActivity**: 不验证状态，仅记录日志

验证成功后，调用 `handler.removeCallbacksAndMessages(null)` 取消所有待执行的超时任务。

---

### 3.3 左右手选择机制

#### 选择状态管理
- **存储位置**: SharedPreferences，键名 `"select_hand"`
- **全局变量**: `Select.select_hand` (静态boolean)
- **默认值**: false (左手)

#### 端口选择逻辑
```java
if (Select.select_hand == true) {
    使用端口 4001 (右手)
} else {
    使用端口 3001 (左手)
}
```


### 3.4 用户交互触发机制

#### 自动控制模式
- **触发**: 点击程序按钮 (ImageView)
- **发送**: 立即发送，无重试
- **特点**: 每个程序对应一个固定的程序号

#### 手动控制模式
- **触发**: 滑块松开事件 (ACTION_UP)
- **发送**: 仅在值变化时发送
- **特点**: 避免拖动过程中频繁发送

---

## 四、指令汇总表

| 指令类型 | 指令格式 | 数据长度 | 发送时机 | 重试机制 | 状态验证 |
|---------|---------|---------|---------|---------|---------|
| 主界面进入 | `AA BB FF CC` | 4字节 | onCreate | 是(800ms) | 否 |
| 自动模式进入 | `AA BB 01 CC` | 4字节 | onCreate | 是(800ms) | 是("state auto\r\n") |
| 自动模式退出 | `AA BB FF CC` | 4字节 | onDestroy | 否 | 否 |
| 自动程序执行 | `AA BB [程序号] FF CC` | 5字节 | 按钮点击 | 否 | 否 |
| 手动模式进入 | `AA BB 02 CC` | 4字节 | onCreate | 是(800ms) | 是("state manual\r\n") |
| 手动模式退出 | `AA BB FF CC` | 4字节 | onDestroy | 否 | 否 |
| 电机控制 | `AA BB [电机] [档位] CC` | 5字节 | 滑块松开 | 否 | 否 |

---

## 五、已知问题与注意事项

### 5.1 代码问题
1. **MainActivity重试计数共享**: `retryCount`是实例变量，左右手重试可能互相影响
2. **AsyncTask已废弃**: NetworkUtils使用已废弃的AsyncTask，建议迁移到现代异步方案
3. **BrainControlActivity未实现**: 脑控功能目前只有界面，无实际功能

### 5.2 通信注意事项
1. **Socket超时**: 每次发送前设置600ms读取超时，可能影响性能
2. **重试机制**: 最大重试1次，网络不稳定时可能失败
3. **状态验证**: 部分指令不验证响应，可能无法确认是否执行成功

### 5.3 数据持久化
1. **SharedPreferences键名**: 使用sliderId作为键名的一部分，可能在不同设备上不一致
2. **数据类型**: 设置值以int存储，但实际使用float，可能存在精度问题

---

## 六、调试建议

### 6.1 日志查看
所有Activity都使用Log.e()记录关键信息:
- 发送的指令（十六进制格式）
- 接收的响应（UTF-8字符串）
- 超时和重试信息
- 错误信息

### 6.2 网络调试
- 检查Socket连接状态
- 验证IP和端口是否正确
- 确认服务器是否正常响应

### 6.3 指令验证
- 使用NetworkUtils.bytesToHex()查看发送的指令
- 检查指令格式是否符合协议
- 验证电机序号和档位值是否在有效范围内

