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
    public class LikeLoL04StateMachineV2Wrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(LikeLoL04.StateMachineV2);
			Utils.BeginObjectRegister(type, L, translator, 0, 8, 4, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RegisterState", _m_RegisterState);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TransitionTo", _m_TransitionTo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetState", _m_GetState);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTransitionDuration", _m_SetTransitionDuration);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsStateRegistered", _m_IsStateRegistered);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetRegisteredStateIds", _m_GetRegisteredStateIds);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnregisterState", _m_UnregisterState);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentStateId", _g_get_CurrentStateId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentState", _g_get_CurrentState);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentTransitionDuration", _g_get_CurrentTransitionDuration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DefaultTransitionDuration", _g_get_DefaultTransitionDuration);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "DefaultTransitionDuration", _s_set_DefaultTransitionDuration);
            
			
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
					
					var gen_ret = new LikeLoL04.StateMachineV2();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to LikeLoL04.StateMachineV2 constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LikeLoL04.StateMachineV2 gen_to_be_invoked = (LikeLoL04.StateMachineV2)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _stateId = LuaAPI.lua_tostring(L, 2);
                    LikeLoL04.IState _state = (LikeLoL04.IState)translator.GetObject(L, 3, typeof(LikeLoL04.IState));
                    
                    gen_to_be_invoked.RegisterState( _stateId, _state );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TransitionTo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LikeLoL04.StateMachineV2 gen_to_be_invoked = (LikeLoL04.StateMachineV2)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _stateId = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.TransitionTo( _stateId );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LikeLoL04.StateMachineV2 gen_to_be_invoked = (LikeLoL04.StateMachineV2)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Update(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LikeLoL04.StateMachineV2 gen_to_be_invoked = (LikeLoL04.StateMachineV2)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _stateId = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetState( _stateId );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTransitionDuration(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LikeLoL04.StateMachineV2 gen_to_be_invoked = (LikeLoL04.StateMachineV2)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _fromStateId = LuaAPI.lua_tostring(L, 2);
                    string _toStateId = LuaAPI.lua_tostring(L, 3);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.SetTransitionDuration( _fromStateId, _toStateId, _duration );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsStateRegistered(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LikeLoL04.StateMachineV2 gen_to_be_invoked = (LikeLoL04.StateMachineV2)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _stateId = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.IsStateRegistered( _stateId );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRegisteredStateIds(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LikeLoL04.StateMachineV2 gen_to_be_invoked = (LikeLoL04.StateMachineV2)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetRegisteredStateIds(  );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnregisterState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LikeLoL04.StateMachineV2 gen_to_be_invoked = (LikeLoL04.StateMachineV2)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _stateId = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.UnregisterState( _stateId );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentStateId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.StateMachineV2 gen_to_be_invoked = (LikeLoL04.StateMachineV2)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.CurrentStateId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.StateMachineV2 gen_to_be_invoked = (LikeLoL04.StateMachineV2)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.CurrentState);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentTransitionDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.StateMachineV2 gen_to_be_invoked = (LikeLoL04.StateMachineV2)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.CurrentTransitionDuration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DefaultTransitionDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.StateMachineV2 gen_to_be_invoked = (LikeLoL04.StateMachineV2)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.DefaultTransitionDuration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DefaultTransitionDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LikeLoL04.StateMachineV2 gen_to_be_invoked = (LikeLoL04.StateMachineV2)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DefaultTransitionDuration = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
