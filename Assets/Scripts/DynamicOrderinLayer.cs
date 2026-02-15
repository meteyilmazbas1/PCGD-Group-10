using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UrbanNinja
{
    public class DynamicOrderinLayer : MonoBehaviour, IDynamicOrderListener
    {
        public static List<IDynamicOrderListener> c_layeredObjects;
        private static GameObject s_owner;
        private void FixedUpdate()
        {
            if (s_owner != gameObject) return;
            SortRelativeToYPosition();
        }
        /// <summary>
        /// Sort the static list based on Y position values
        /// of the list members. Set the order in layer 
        /// accordingly.
        /// </summary>
        private void SortRelativeToYPosition()
        {
            if (c_layeredObjects == null || c_layeredObjects.Count == 0) return;
            
            // Remove any null entries from destroyed objects
            c_layeredObjects.RemoveAll(item => item == null || (item is MonoBehaviour mb && mb == null));
            
            if (c_layeredObjects.Count > 1)
            {
                c_layeredObjects.Sort(Compare);
            }
            for (int i = c_layeredObjects.Count - 1; i >= 0; i--)
            {
                c_layeredObjects[i].SetOrderInLayer(i);
            }
        }

        private List<SpriteRenderer> _sprites;
        private void Awake()
        {
            ResolveStaticOwner();
            c_layeredObjects.Add(this);
            _sprites = GetComponentsInChildren<SpriteRenderer>().ToList();
        }
        
        private void OnDestroy()
        {
            if (c_layeredObjects != null)
            {
                c_layeredObjects.Remove(this);
            }
        }
        /// <summary>
        /// Sets the first in line to call Awake as
        /// owner for the static member.
        /// </summary>
        private void ResolveStaticOwner()
        {
            if (s_owner == null)
            {
                s_owner = gameObject;
                c_layeredObjects = new();
            }
        }

        /// <summary>
        /// Sets the order in layer for all
        /// SpriteRenderers in _sprites list.
        /// </summary>
        /// <param name="layerOrder"></param>
        public void SetOrderInLayer(int layerOrder)
        {
            foreach (SpriteRenderer spriteRenderer in _sprites)
            {
                spriteRenderer.sortingOrder = layerOrder;
            }
        }

        public float GetPositionY()
        {
            return transform.position.y;
        }
        /// <summary>
        /// Comparer delegate to use for list sorting.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(IDynamicOrderListener x, IDynamicOrderListener y)
        {
            if (x.GetPositionY() > y.GetPositionY()) return -1;
            if (x.GetPositionY() == y.GetPositionY()) return 0;
            if (x.GetPositionY() < y.GetPositionY()) return 1;
            return 0;
        }
    }
}
