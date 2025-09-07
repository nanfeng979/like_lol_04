## 1.避免在 Update 中做昂贵或重复操作
❌ 坏例子：
```csharp
void Update()
{
    GameObject player = GameObject.Find("Player"); // 每帧查找！
    Rigidbody rb = player.GetComponent<Rigidbody>(); // 每帧获取组件！
    if (Input.GetKeyDown(KeyCode.Space))
    {
        rb.AddForce(Vector3.up * 10f);
    }
}
```
✅ 好例子：
```csharp
private Rigidbody playerRigidbody;

void Start()
{
    GameObject player = GameObject.Find("Player"); // 只找一次
    playerRigidbody = player.GetComponent<Rigidbody>(); // 缓存组件
}

void Update()
{
    if (Input.GetKeyDown(KeyCode.Space))
    {
        playerRigidbody.AddForce(Vector3.up * 10f);
    }
}
```
💡 规范：组件引用、对象查找、资源加载等操作应在 Start() 或 Awake() 中缓存，避免每帧重复执行。 

## 2.避免“上帝类”（God Class）
❌ 坏例子：
```csharp
public class GameManager : MonoBehaviour
{
    // 控制UI、音效、存档、敌人生成、玩家状态、网络同步...
    public void UpdateScore() { ... }
    public void PlaySound() { ... }
    public void SaveGame() { ... }
    public void SpawnEnemy() { ... }
    public void HandleInput() { ... }
    // 5000行代码...
}
```
✅ 好例子：
```csharp
public class ScoreManager : MonoBehaviour { ... }
public class AudioManager : MonoBehaviour { ... }
public class SaveManager : MonoBehaviour { ... }
public class EnemySpawner : MonoBehaviour { ... }
public class InputManager : MonoBehaviour { ... }

// GameManager 只负责协调或事件分发
public class GameManager : MonoBehaviour
{
    private ScoreManager scoreMgr;
    private AudioManager audioMgr;
    
    void Start()
    {
        scoreMgr = FindObjectOfType<ScoreManager>();
        audioMgr = FindObjectOfType<AudioManager>();
    }
}
```
💡 规范：单一职责原则（SRP）。每个类只负责一个功能模块，提高可读性和可测试性。 

三、避免硬编码（Hardcoding）
❌ 坏例子：
```csharp
void Jump()
{
    rb.AddForce(Vector3.up * 500f); // 500 是什么？魔法数字！
}

void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Enemy")) // Tag字符串硬编码，易拼错
    {
        Destroy(gameObject);
    }
}
```
✅ 重构后：
```csharp
[SerializeField] private float jumpForce = 500f; // Inspector可配置
[SerializeField] private string enemyTag = "Enemy"; // 可配置，避免拼写错误

void Jump()
{
    rb.AddForce(Vector3.up * jumpForce);
}

void OnTriggerEnter(Collider other)
{
    if (other.CompareTag(enemyTag))
    {
        Destroy(gameObject);
    }
}
```
💡 规范：所有可变参数应通过 [SerializeField] 暴露给 Inspector，或使用 const / static readonly 定义常量。避免“魔法数字/字符串”。

## 4、避免直接 GetComponent 多次调用
❌ 坏例子：
```csharp
void Update()
{
    GetComponent<Renderer>().material.color = Color.red;
    GetComponent<Rigidbody>().velocity = Vector3.zero;
}
```
✅ 好例子：
```csharp
private Renderer myRenderer;
private Rigidbody myRigidbody;

void Awake()
{
    myRenderer = GetComponent<Renderer>();
    myRigidbody = GetComponent<Rigidbody>();
}

void Update()
{
    myRenderer.material.color = Color.red;
    myRigidbody.velocity = Vector3.zero;
}
```
💡 规范：组件获取应缓存，GetComponent<T>() 是反射操作，性能开销大。

## 5、避免 Update 中做复杂逻辑或嵌套判断
❌ 坏例子：
```csharp
void Update()
{
    if (isPlayerAlive)
    {
        if (hasWeapon)
        {
            if (Input.GetMouseButton(0))
            {
                if (Time.time > nextFireTime)
                {
                    Shoot();
                    nextFireTime = Time.time + fireRate;
                    if (ammo <= 0) Reload();
                }
            }
        }
    }
}
```
✅ 好例子：
```csharp
void Update()
{
    HandleShooting();
}

private void HandleShooting()
{
    if (!CanShoot()) return;
    
    if (Input.GetMouseButton(0) && IsTimeToFire())
    {
        Shoot();
        UpdateNextFireTime();
        TryReloadIfEmpty();
    }
}

private bool CanShoot() => isPlayerAlive && hasWeapon;
private bool IsTimeToFire() => Time.time > nextFireTime;
private void UpdateNextFireTime() => nextFireTime = Time.time + fireRate;
private void TryReloadIfEmpty()
{
    if (ammo <= 0) Reload();
}
```
💡 规范：将复杂逻辑拆分成小函数，提高可读性和可测试性。函数命名应表达意图。