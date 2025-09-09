using UnityEngine;
using NewActionSystem.Core;
using NewActionSystem.States;
using NewActionSystem.Commands;

namespace NewActionSystem.Player
{
    /// <summary>
    /// 玩家控制器：管理玩家的状态、动画和行为
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        #region Serialized Fields
        
        [Header("移动设置")]
        [SerializeField, Tooltip("移动速度")]
        private float moveSpeed = 5.0f;
        
        [SerializeField, Tooltip("旋转速度")]
        private float rotationSpeed = 10.0f;
        
        [Header("攻击设置")]
        [SerializeField, Tooltip("攻击范围")]
        private float attackRange = 2.0f;
        
        [SerializeField, Tooltip("攻击伤害")]
        private float attackDamage = 10.0f;
        
        #endregion
        
        #region Private Fields
        
        /// <summary>
        /// 状态机
        /// </summary>
        private StateMachine stateMachine;
        
        /// <summary>
        /// 命令调用器
        /// </summary>
        private CommandInvoker commandInvoker;
        
        /// <summary>
        /// 动画控制器
        /// </summary>
        private Animator animator;
        
        /// <summary>
        /// 刚体组件
        /// </summary>
        private Rigidbody rigidBody;
        
        /// <summary>
        /// 移动目标位置
        /// </summary>
        private Vector3 moveTarget;
        
        /// <summary>
        /// 攻击目标
        /// </summary>
        private Transform attackTarget;
        
        /// <summary>
        /// 是否有移动目标
        /// </summary>
        private bool hasMoveTarget;
        
        /// <summary>
        /// 移动输入向量
        /// </summary>
        private Vector3 moveInput;
        
        #endregion
        
        #region Unity Lifecycle
        
        void Awake()
        {
            InitializeComponents();
            InitializeStateMachine();
            InitializeCommandInvoker();
        }
        
        void Start()
        {
            // 初始状态设为待机
            stateMachine.TransitionTo<IdleState>();
        }
        
        void Update()
        {
            HandleInput();
            stateMachine.Update();
        }
        
        void FixedUpdate()
        {
            // 在FixedUpdate中处理物理相关的移动
            if (hasMoveTarget)
            {
                MoveTowardsTarget();
            }
        }
        
        #endregion
        
        #region Initialization
        
        /// <summary>
        /// 初始化组件引用
        /// </summary>
        private void InitializeComponents()
        {
            animator = GetComponent<Animator>();
            rigidBody = GetComponent<Rigidbody>();
            
            if (animator == null)
            {
                Debug.LogError("PlayerController: 未找到Animator组件");
            }
            
            if (rigidBody == null)
            {
                Debug.LogError("PlayerController: 未找到Rigidbody组件");
            }
        }
        
        /// <summary>
        /// 初始化状态机
        /// </summary>
        private void InitializeStateMachine()
        {
            stateMachine = new StateMachine();
            
            // 注册所有状态
            stateMachine.RegisterState(new IdleState(stateMachine, this));
            stateMachine.RegisterState(new MoveState(stateMachine, this));
            stateMachine.RegisterState(new AttackState(stateMachine, this));
        }
        
        /// <summary>
        /// 初始化命令调用器
        /// </summary>
        private void InitializeCommandInvoker()
        {
            commandInvoker = new CommandInvoker();
        }
        
        #endregion
        
        #region Input Handling
        
        /// <summary>
        /// 处理输入
        /// </summary>
        private void HandleInput()
        {
            // 处理移动输入
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            moveInput = new Vector3(horizontal, 0, vertical).normalized;
            
            // 右键点击移动
            if (Input.GetMouseButtonDown(1))
            {
                HandleRightClick();
            }
            
            // 左键点击攻击
            if (Input.GetMouseButtonDown(0))
            {
                HandleLeftClick();
            }
            
            // 停止命令
            if (Input.GetKeyDown(KeyCode.S))
            {
                ExecuteStopCommand();
            }
        }
        
        /// <summary>
        /// 处理右键点击（移动）
        /// </summary>
        private void HandleRightClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 targetPosition = hit.point;
                MoveCommand moveCommand = new MoveCommand(this, targetPosition);
                commandInvoker.ExecuteCommand(moveCommand);
            }
        }
        
        /// <summary>
        /// 处理左键点击（攻击）
        /// </summary>
        private void HandleLeftClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Transform target = hit.collider.transform;
                AttackCommand attackCommand = new AttackCommand(this, target);
                commandInvoker.ExecuteCommand(attackCommand);
            }
        }
        
        #endregion
        
        #region Public Methods - State Query
        
        /// <summary>
        /// 检查是否有移动输入
        /// </summary>
        /// <returns>是否有移动输入</returns>
        public bool HasMoveInput()
        {
            return moveInput.magnitude > 0.1f || hasMoveTarget;
        }
        
        /// <summary>
        /// 检查是否有攻击输入
        /// </summary>
        /// <returns>是否有攻击输入</returns>
        public bool HasAttackInput()
        {
            return attackTarget != null;
        }
        
        #endregion
        
        #region Public Methods - Animation
        
        /// <summary>
        /// 播放指定动画
        /// </summary>
        /// <param name="animationName">动画名称</param>
        public void PlayAnimation(string animationName)
        {
            if (animator != null)
            {
                animator.Play(animationName);
                Debug.Log($"播放动画: {animationName}");
            }
        }
        
        #endregion
        
        #region Public Methods - Movement
        
        /// <summary>
        /// 设置移动目标
        /// </summary>
        /// <param name="target">目标位置</param>
        public void SetMoveTarget(Vector3 target)
        {
            moveTarget = target;
            hasMoveTarget = true;
        }
        
        /// <summary>
        /// 停止移动
        /// </summary>
        public void StopMovement()
        {
            hasMoveTarget = false;
            moveInput = Vector3.zero;
            rigidBody.velocity = Vector3.zero;
        }
        
        /// <summary>
        /// 处理移动逻辑
        /// </summary>
        public void HandleMovement()
        {
            // 键盘输入移动优先级更高
            if (moveInput.magnitude > 0.1f)
            {
                Vector3 movement = moveInput * moveSpeed;
                rigidBody.velocity = new Vector3(movement.x, rigidBody.velocity.y, movement.z);
                
                // 朝向移动方向
                if (movement != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(movement);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
        }
        
        /// <summary>
        /// 向目标位置移动
        /// </summary>
        private void MoveTowardsTarget()
        {
            Vector3 direction = (moveTarget - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, moveTarget);
            
            // 如果到达目标位置，停止移动
            if (distance < 0.5f)
            {
                hasMoveTarget = false;
                rigidBody.velocity = Vector3.zero;
                return;
            }
            
            // 移动向目标位置
            Vector3 movement = direction * moveSpeed;
            rigidBody.velocity = new Vector3(movement.x, rigidBody.velocity.y, movement.z);
            
            // 朝向目标方向
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        
        #endregion
        
        #region Public Methods - Attack
        
        /// <summary>
        /// 设置攻击目标
        /// </summary>
        /// <param name="target">攻击目标</param>
        public void SetAttackTarget(Transform target)
        {
            attackTarget = target;
        }
        
        /// <summary>
        /// 造成伤害
        /// </summary>
        public void DealDamage()
        {
            if (attackTarget == null) return;
            
            float distance = Vector3.Distance(transform.position, attackTarget.position);
            if (distance <= attackRange)
            {
                Debug.Log($"对 {attackTarget.name} 造成 {attackDamage} 点伤害");
                
                // 这里可以添加实际的伤害逻辑
                // 例如：attackTarget.GetComponent<Health>()?.TakeDamage(attackDamage);
            }
            
            // 攻击完成后清除目标
            attackTarget = null;
        }
        
        #endregion
        
        #region Public Methods - Commands
        
        /// <summary>
        /// 停止所有动作
        /// </summary>
        public void StopAllActions()
        {
            StopMovement();
            SetAttackTarget(null);
        }
        
        /// <summary>
        /// 执行停止命令
        /// </summary>
        private void ExecuteStopCommand()
        {
            StopCommand stopCommand = new StopCommand(this);
            commandInvoker.ExecuteCommand(stopCommand);
        }
        
        #endregion
        
        #region Debug
        
        void OnDrawGizmosSelected()
        {
            // 绘制攻击范围
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            
            // 绘制移动目标
            if (hasMoveTarget)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(moveTarget, 0.3f);
                Gizmos.DrawLine(transform.position, moveTarget);
            }
        }
        
        #endregion
    }
}
