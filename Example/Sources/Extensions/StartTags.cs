using System.Collections.Generic;
using UnityEngine;

namespace CeresECL.Example
{
    class StartTags : ExtendedBehaviour
    {
        [SerializeField] List<Tag> tags;

        protected override void Init()
        {
	        for (int i = 0; i < tags.Count; i++)
		        Entity.Tags.Add(tag[i]);
        }
    }
}