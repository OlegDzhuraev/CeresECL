using System;
using UnityEngine;

namespace CeresECL.Misc 
{
	[Serializable]
	public sealed class SerializableType : ISerializationCallbackReceiver 
	{
		public Type Type
		{
			get => type;
			set 
			{
				if (value != null && !value.IsClass)
					throw new ArgumentException($"'{value.FullName}' is not a class type.", nameof(value));

				type = value;
				classReference = GetClassReference(value);
			}
		}
		
		[SerializeField] string classReference;
		Type type;
		
		public SerializableType() { }
		public SerializableType(Type type) => Type = type;

		public static string GetClassReference(Type type) => type != null ? type.FullName + ", " + type.Assembly.GetName().Name : "";

		void ISerializationCallbackReceiver.OnAfterDeserialize() 
		{
			if (!string.IsNullOrEmpty(classReference))
			{
				type = Type.GetType(classReference);

				if (type == null)
					Debug.LogWarning($"'{classReference}' is set, but class type was not found.");
			}
			else 
			{
				type = null;
			}
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize() { }
		
		public static implicit operator Type(SerializableType type) => type.Type;
		public static implicit operator SerializableType(Type type) => new SerializableType(type);
		public override string ToString() => Type != null ? Type.FullName : "(None)";

		public override bool Equals(object obj)
		{
			if (obj is Type otherType)
				return type == otherType;
			if (obj is SerializableType otherSerializableType)
				return otherSerializableType.Type == Type;
			
			return false;
		}
	}
}
