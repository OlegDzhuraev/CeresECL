using System;
using System.Collections.Generic;

namespace CeresECL
{
    public class Tags
    {
        /// <summary> Subscribers of event when tag was added. Event doesnt raised when second same tag added.</summary>
        readonly Dictionary<Tag, List<Action>> addSubscribers = new Dictionary<Tag, List<Action>>();

        /// <summary> Subscribers of event when tag was removed. Event is being raised when last tag of some type was removed.</summary>
        readonly Dictionary<Tag, List<Action>> removeSubscribers = new Dictionary<Tag, List<Action>>();

        readonly Dictionary<Tag, int> addedTags = new Dictionary<Tag, int>();

        public void Add(params Tag[] tags)
        {
            foreach (var tag in tags)
                if (HaveAny(tag))
                    addedTags[tag]++;
                else
                    FirstAddTag(tag);
        }

        public void AddOnce(params Tag[] tags)
        {
            foreach (var tag in tags)
                if (!HaveAny(tag))
                    FirstAddTag(tag);
        }

        void FirstAddTag(Tag tag)
        {
            addedTags.Add(tag, 1);
            InvokeActions(tag, addSubscribers);
        }

        public void Remove(params Tag[] tags)
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

        void InvokeActions(Tag tag, Dictionary<Tag, List<Action>> dict)
        {
            if (dict.ContainsKey(tag))
                foreach (var action in dict[tag])
                    action?.Invoke();
        }

        public bool Have(Tag tag) => addedTags.ContainsKey(tag);

        public bool HaveAny(params Tag[] tags)
        {
            foreach (var tag in tags)
                if (Have(tag))
                    return true;

            return false;
        }

        public bool HaveAll(params Tag[] tags)
        {
            foreach (var tag in tags)
                if (!Have(tag))
                    return false;

            return true;
        }

        public void SubscribeToAdd(Tag tag, Action action) => AddToDict(addSubscribers, tag, action);
        public void UnsubscribeFromAdd(Tag tag, Action action) => DelFromDict(addSubscribers, tag, action);

        public void SubscribeToRemove(Tag tag, Action action) => AddToDict(removeSubscribers, tag, action);
        public void UnsubscribeFromRemove(Tag tag, Action action) => DelFromDict(removeSubscribers, tag, action);

        static void AddToDict(Dictionary<Tag, List<Action>> dict, Tag tag, Action action)
        {
            if (!dict.ContainsKey(tag))
                dict[tag] = new List<Action>();

            dict[tag].Add(action);
        }

        static void DelFromDict(Dictionary<Tag, List<Action>> dict, Tag tag, Action action)
        {
            if (dict.ContainsKey(tag))
                dict[tag].Remove(action);
        }

        /// <summary> Only for editor debug. </summary>
        public Dictionary<Tag, int> GetTagsCopy() => new Dictionary<Tag, int>(addedTags);
    }
}