
## 登入功能描述

### 规定了一下目录结构和代码样式

## 一、基础命名风格

### 1. 禁止特殊符号
- 命名不能以下划线 `_` 或美元符号 `$` 开头或结尾  
  ❌ 反例：`_name`, `name$`

### 2. 禁止拼音混合/中文
- 严禁拼音与英文混合  
  ❌ 反例：`getPingfenByName()`
- 避免纯拼音命名

### 3. 类名规范
- 使用 `UpperCamelCase`（大驼峰）  
  ✅ 正例：`UserService`
- 例外：`DO/BO/DTO/VO/AO/PO/UID`  
  ✅ 正例：`UserDO`

### 4. 方法/变量名
- 统一使用 `lowerCamelCase`（小驼峰）  
  ✅ 正例：`getHttpMessage()`, `localValue`

### 5. 常量名
- 全部大写，单词间用下划线分隔  
  ✅ 正例：`MAX_STOCK_COUNT`  
  ❌ 反例：`maxCount`

## 二、特殊场景命名

### 1. 特定类型命名
| 类型       | 规则                     | 示例               |
|------------|--------------------------|--------------------|
| 抽象类     | `Abstract`/`Base` 开头   | `AbstractService`  |
| 异常类     | `Exception` 结尾         | `NotFoundException`|
| 测试类     | 被测类名+`Test`          | `UserServiceTest`  |

### 2. 其他规范
- **数组定义**：类型与中括号紧邻  
  ✅ 正例：`int[] array`  
  ❌ 反例：`String args[]`

- **布尔类型**：POJO 中不加 `is` 前缀  
  ❌ 反例：`isDeleted`（易引发序列化错误）

- **包名**：全小写，单数形式  
  ✅ 正例：`com.alibaba.util`

## 三、分层命名规约

### Service/DAO 层
| 操作类型   | 前缀        | 示例           |
|------------|-------------|----------------|
| 查询单条   | `get`       | `getUser()`    |
| 查询多条   | `list`      | `listOrders()` |
| 统计       | `count`     | `countUsers()` |
| 新增       | `save`/`insert` | `saveOrder()` |
| 删除       | `remove`/`delete` | `deleteUser()` |
| 修改       | `update`    | `updateConfig()`|

### 领域模型
| 类型       | 后缀  | 示例       |
|------------|-------|------------|
| 数据对象   | `DO`  | `UserDO`   |
| 数据传输对象 | `DTO` | `OrderDTO` |
| 展示对象   | `VO`  | `UserVO`   |

> 📌 POJO 是 `DO/DTO/BO/VO` 的统称，禁止使用 `xxxPOJO` 命名

## 四、其他重要规则

### 1. 杜绝不规范缩写
❌ 反例：`AbstractClass` → `AbsClass`

### 2. 设计模式体现
✅ 正例：
- `OrderFactory`（工厂模式）
- `LoginProxy`（代理模式）

### 3. 接口命名规范
- 实现类加 `Impl` 后缀  
  ✅ 正例：`CacheServiceImpl` 实现 `CacheService`
- 能力型接口使用 `-able`  
  ✅ 正例：`Runnable`, `Translatable`

### 4. 枚举规范
- 类名加 `Enum` 后缀
- 成员全大写+下划线  
  ✅ 正例：
  ```java
  public enum StatusEnum {
      SUCCESS, FAILED
  }

#### 以上都还没有捏最好能遵守辣，不能遵守也没事0v0