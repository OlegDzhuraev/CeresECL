using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CeresECL
{
    public sealed class Logics : Container
    {
        readonly IRunLogic[] runLogics = new IRunLogic[CeresSettings.MaxEntityLogics];
        int logicsAdded;
        
        public Logics(Entity entity) : base(entity) { }
        
        public override void Run()
        {
            for (int i = 0; i < logicsAdded; i++)
                runLogics[i].Run();
        }

        public void Add<T>() where T : Logic, new()
        {
            if (logicsAdded == CeresSettings.MaxEntityLogics)
            {
                Debug.LogWarning("You're trying to add more logics than it is allowed. Change CeresSettings parameters to allow it.");
                return;
            }
            
            for (var i = 0; i < logicsAdded; i++)
                if (runLogics[i] is T)
                    return;

            var newLogic = new T
            {
                Entity = Entity
            };

            if (newLogic is IInitLogic initLogic)
                initLogic.Init();
            
            if (newLogic is IRunLogic runLogic)
            {
                runLogics[logicsAdded] = runLogic;
                logicsAdded++;
            }
        }

        /// <summary> Used only for editor scripting. </summary>
        public List<Logic> GetListEditor()
        {
            var list = new List<Logic>();
            
            for (var i = 0; i < logicsAdded; i++)
                list.Add(runLogics[i] as Logic);
            
            return list;
        }
        
        /// <summary> Method allows you inject any data to all Entity logics. </summary>
        public void Inject (object data)
        {
            var dataType = data.GetType();
            
            var bindingAttribute = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var ignoreAttribute = typeof(CeresIgnoreInjectAttribute);
            
            foreach (var logic in runLogics)
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