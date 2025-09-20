#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class LikeLoL04LOLGameObjectWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(LikeLoL04.LOLGameObject);
			Utils.BeginObjectRegister(type, L, translator, 0, 9, 14, 9);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HandleMoveToPosition", _m_HandleMoveToPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InteractWithTarget", _m_InteractWithTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InteractWithPosition", _m_InteractWithPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HandleRotation", _m_HandleRotation);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsTargetInAttackRange", _m_IsTargetInAttackRange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AttackTargetListener", _m_AttackTargetListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BeAttack", _m_BeAttack);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddBuff", _m_AddBuff);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearBuffs", _m_ClearBuffs);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "animator", _g_get_animator);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "StateMachine", _g_get_StateMachine);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BuffManager", _g_get_BuffManager);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BuffList", _g_get_BuffList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Target", _g_get_Target);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TargetPosition", _g_get_TargetPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsMoveToTarget", _g_get_IsMoveToTarget);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MoveSpeed", _g_get_MoveSpeed);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RotationDuration", _g_get_RotationDuration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Camp", _g_get_Camp);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DefaultStateId", _g_get_DefaultStateId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MoveStateId", _g_get_MoveStateId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AttackStateId", _g_get_AttackStateId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AttackRange", _g_get_AttackRange);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Target", _s_set_Target);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "TargetPosition", _s_set_TargetPosition);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MoveSpeed", _s_set_MoveSpeed);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RotationDuration", _s_set_RotationDuration);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Camp", _s_set_Camp);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DefaultStateId", _s_set_DefaultStateId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MoveStateId", _s_set_MoveStateId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AttackStateId", _s_set_AttackStateId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AttackRange", _s_set_AttackRange);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new LikeLoL04.LOLGameObject();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to LikeLoL04.LOLGameObject constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HandleMoveToPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.HandleMoveToPosition(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InteractWithTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    LikeLoL04.LOLGameObject _target = (LikeLoL04.LOLGameObject)translator.GetObject(L, 2, typeof(LikeLoL04.LOLGameObject));
                    
                    gen_to_be_invoked.InteractWithTarget( _target );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InteractWithPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _targetPos;translator.Get(L, 2, out _targetPos);
                    
                    gen_to_be_invoked.InteractWithPosition( _targetPos );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HandleRotation(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _targetPos;translator.Get(L, 2, out _targetPos);
                    
                    gen_to_be_invoked.HandleRotation( _targetPos );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsTargetInAttackRange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    LikeLoL04.LOLGameObject _target = (LikeLoL04.LOLGameObject)translator.GetObject(L, 2, typeof(LikeLoL04.LOLGameObject));
                    
                        var gen_ret = gen_to_be_invoked.IsTargetInAttackRange( _target );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AttackTargetListener(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.AttackTargetListener(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BeAttack(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.BeAttack(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddBuff(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    LikeLoL04.Buff _buff = (LikeLoL04.Buff)translator.GetObject(L, 2, typeof(LikeLoL04.Buff));
                    
                        var gen_ret = gen_to_be_invoked.AddBuff( _buff );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearBuffs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ClearBuffs(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_animator(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.animator);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_StateMachine(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.StateMachine);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BuffManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BuffManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BuffList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.BuffList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Target(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Target);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TargetPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.TargetPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsMoveToTarget(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsMoveToTarget);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MoveSpeed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.MoveSpeed);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RotationDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.RotationDuration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Camp(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Camp);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DefaultStateId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.DefaultStateId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MoveStateId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.MoveStateId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AttackStateId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.AttackStateId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AttackRange(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.AttackRange);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Target(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Target = (LikeLoL04.LOLGameObject)translator.GetObject(L, 2, typeof(LikeLoL04.LOLGameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TargetPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.TargetPosition = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MoveSpeed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MoveSpeed = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RotationDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RotationDuration = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Camp(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                Camp gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.Camp = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DefaultStateId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DefaultStateId = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MoveStateId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MoveStateId = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AttackStateId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AttackStateId = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AttackRange(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.LOLGameObject gen_to_be_invoked = (LikeLoL04.LOLGameObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AttackRange = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
