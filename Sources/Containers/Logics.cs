using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CeresECL
{
    public sealed class Logics : Container
    {
        readonly List<Logic> logics = new List<Logic>();
        
        public Logics(Entity entity) : base(entity) { }
        
        public override void Run()
        {
            foreach (var logic in logics)
                if (logic is IRunLogic runLogic)
                    runLogic.Run();
        }
        
        public void Add<T>() where T : Logic, new()
        {
            foreach (var logic in logics)
                if (logic is T)
                    return;

            var newLogic = new T
            {
                Entity = Entity
            };

            if (Application.isPlaying && newLogic is IInitLogic initLogic)
                initLogic.Init(); // todo maybe make first init in update or smth
            
            logics.Add(newLogic);
        }
        
        /// <summary> Used only for editor scripting. </summary>
        public List<Logic> GetListEditor()
        {
            var list = new List<Logic>();

            for (var i = 0; i < logics.Count; i++)
                list.Add(logics[i]);
            
            return list;
        }
        
        /// <summary> Method allows you inject any data to all Entity logics. </summary>
        public void Inject (object data)
        {
            var dataType = data.GetType();
            
            var bindingAttribute = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var ignoreAttribute = typeof(CeresIgnoreInjectAttribute);
            
            foreach (var logic in logics)
            {  
                var logicType = logic.GetType();

                foreach (var field in logicType.GetFields(bindingAttribute))
                {
                    if (field.IsStatic || Attribute.IsDefined(field, ignoreAttribute))
                        continue;
                    
                    if (field.FieldType.IsAssignableFrom(dataType))
                    {
                        field.SetValue(logic, data);
                        break;
                    }
                }
            }
        }
    }
}