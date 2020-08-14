using System;
using System.Collections.Generic;

namespace CeresECL
{
    public sealed class Tags
    {
        /// <summary> Subscribers of event when tag was added. Event doesnt raised when second same tag added.</summary>
        readonly Dictionary<IntTag, List<Action>> addSubscribers = new Dictionary<IntTag, List<Action>>();

        /// <summary> Subscribers of event when tag was removed. Event is being raised when last tag of some type was removed.</summary>
        readonly Dictionary<IntTag, List<Action>> removeSubscribers = new Dictionary<IntTag, List<Action>>();

        readonly Dictionary<IntTag, int> addedTags = new Dictionary<IntTag, int>();
 
        public void Add(params IntTag[] tags)
        {
            foreach (var tag in tags)
                if (HaveAny(tag))
                    addedTags[tag]++;
                else
                    FirstAddTag(tag);
        }

        public void AddOnce(params IntTag[] tags)
        {
            foreach (var tag in tags)
                if (!HaveAny(tag))
                    FirstAddTag(tag);
        }

        void FirstAddTag(IntTag tag)
        {
            addedTags.Add(tag, 1);
            InvokeActions(tag, addSubscribers);
        }

        public void Remove(params IntTag[] tags)
        {
            foreach (var tag in tags)
            {
                if (!HaveAny(tag))
                    return;

                addedTags[tag]--;

                if (addedTags[tag] > 0)
                    continue;

                addedTags.Remove(tag);
                InvokeActions(tag, removeSubscribers);
            }
        }

        void InvokeActions(IntTag tag, Dictionary<IntTag, List<Action>> dict)
        {
            if (dict.ContainsKey(tag))
                foreach (var action in dict[tag])
                    action?.Invoke();
        }

        public bool Have(IntTag tag) => addedTags.ContainsKey(tag);

        public bool HaveAny(params IntTag[] tags)
        {
            foreach (var tag in tags)
                if (Have(tag))
                    return true;

            return false;
        }

        public bool HaveAll(params IntTag[] tags)
        {
            foreach (var tag in tags)
                if (!Have(tag))
                    return false;

            return true;
        }

        public void SubscribeToAdd(IntTag tag, Action action) => AddToDict(addSubscribers, tag, action);
        public void UnsubscribeFromAdd(IntTag tag, Action action) => DelFromDict(addSubscribers, tag, action);

        public void SubscribeToRemove(IntTag tag, Action action) => AddToDict(removeSubscribers, tag, action);
        public void UnsubscribeFromRemove(IntTag tag, Action action) => DelFromDict(removeSubscribers, tag, action);

        static void AddToDict(Dictionary<IntTag, List<Action>> dict, IntTag tag, Action action)
        {
            if (!dict.ContainsKey(tag))
                dict[tag] = new List<Action>();

            dict[tag].Add(action);
        }

        static void DelFromDict(Dictionary<IntTag, List<Action>> dict, IntTag tag, Action action)
        {
            if (dict.ContainsKey(tag))
                dict[tag].Remove(action);
        }

        /// <summary> Only for editor debug. </summary>
        public Dictionary<IntTag, int> GetTagsCopy() => new Dictionary<IntTag, int>(addedTags);
    }
}