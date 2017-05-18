/*****************************************************************
 * 版权所有 (C) Lave.Zhang@outlook.com 2012
 * 本源代码仅供学习研究之用，不得用于商业目的。
 ****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetroGraphApp
{
    /// <summary>
    /// 乘车路线。
    /// 该类表示从一个站定到另一个站点的乘车路线。例如：南京西路->人民广场->南京东路。
    /// </summary>
    public sealed class MetroPath
    {
        #region 字段区域

        private List<MetroLink> m_links = new List<MetroLink>();

        #endregion

        #region 属性区域

        /// <summary>
        /// 获取路径列表。该属性始终不为空引用。
        /// </summary>
        public List<MetroLink> Links
        {
            get { return m_links; }
        }

        /// <summary>
        /// 获取换乘次数。
        /// </summary>
        public int Transfers
        {
            get
            {
                if (m_links.Count < 2) return 0;

                int count = 0;
                var line = Links[0].Line;
                for (int i = 1; i < Links.Count; i++)
                {
                    if (Links[i].Line != line)
                    {
                        line = Links[i].Line;
                        count++;
                    }
                }
                return count;
            }
        }

        #endregion

        #region 方法区域

        /// <summary>
        /// 判断是否包含指定的站点。
        /// </summary>
        /// <param name="node">目标站点。</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">如果node为空引用，则跑出该异常。</exception>
        public bool ContainsNode(MetroNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            return Links.FirstOrDefault(c => c.From == node || c.To == node) != null;
        }

        /// <summary>
        /// 最佳一条新的Link，并返回一条新的路线。
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">如果link为空引用，则跑出该异常。</exception>
        public MetroPath Append(MetroLink link)
        {
            if (link == null) throw new ArgumentNullException("link");

            MetroPath newPath = new MetroPath();
            newPath.Links.AddRange(this.Links);
            newPath.Links.Add(link);
            return newPath;
        }

        /// <summary>
        /// 合并路线。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">如果path为空引用，则跑出该异常。</exception>
        public MetroPath Merge(MetroPath path)
        {
            if (path == null) throw new ArgumentNullException("path");

            MetroPath newPath = new MetroPath();
            newPath.Links.AddRange(this.Links);
            newPath.Links.AddRange(path.Links);
            return newPath;
        }

        #endregion
    }
}