/*****************************************************************
 * 版权所有 (C) Lave.Zhang@outlook.com 2012
 * 本源代码仅供学习研究之用，不得用于商业目的。
 ****************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Xml;

namespace MetroGraphApp
{
    /// <summary>
    /// 地铁线路图控件类。
    /// </summary>
    public partial class MetroGraphView : UserControl
    {
        #region 构造区域

        /// <summary>
        /// 构造函数。
        /// </summary>
        public MetroGraphView()
        {
            InitializeComponent();

            //优化绘图
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        #endregion

        #region 字段区域

        private MetroGraph m_graph = new MetroGraph();
        private int m_scrollX;
        private int m_scrollY;
        private float m_zoomScale = 1;
        private MetroNode m_startNode;
        private MetroNode m_endNode;
        private MetroPath m_curPath;

        #endregion

        #region 属性区域

        /// <summary>
        /// 获取地铁线路图。该属性始终不为空引用。
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MetroGraph Graph
        {
            get { return m_graph; }
        }

        /// <summary>
        /// 获取或设置当前水平滚动量。
        /// </summary>
        [Description("当前水平滚动量。"), DefaultValue(0)]
        public int ScrollX
        {
            get { return m_scrollX; }
            set
            {
                if (m_scrollX != value)
                {
                    m_scrollX = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 获取或设置当前垂直滚动量。
        /// </summary>
        [Description("当前垂直滚动量。"), DefaultValue(0)]
        public int ScrollY
        {
            get { return m_scrollY; }
            set
            {
                if (m_scrollY != value)
                {
                    m_scrollY = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 获取或设置当前缩放比例。
        /// </summary>
        [Description("当前缩放比例。"), DefaultValue(1)]
        public float ZoomScale
        {
            get { return m_zoomScale; }
            set
            {
                if (m_zoomScale != value)
                {
                    m_zoomScale = Math.Max(value, 0.1f);
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 获取或设置起点。
        /// </summary>
        [Browsable(false)]
        public MetroNode StartNode
        {
            get { return m_startNode; }
            set { m_startNode = value; }
        }

        /// <summary>
        /// 获取或设置终点。
        /// </summary>
        [Browsable(false)]
        public MetroNode EndNode
        {
            get { return m_endNode; }
            set { m_endNode = value; }
        }

        /// <summary>
        /// 获取或设置当前乘车路线。该属性始终不为空引用。
        /// </summary>
        [Browsable(false)]
        public MetroPath CurPath
        {
            get
            {
                if (m_curPath == null)
                    m_curPath = new MetroPath();
                return m_curPath;
            }
            set { m_curPath = value; }
        }

        #endregion

        #region 方法区域

        /// <summary>
        /// 将地铁线路图保存到指定文件。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        public void SaveToFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<?xml version=\"1.0\" encoding=\"gb2312\"?><MetroGraph/>");

            var graphNode = doc.DocumentElement;
            AddAtrribute(graphNode, "ScrollX", this.ScrollX.ToString());
            AddAtrribute(graphNode, "ScrollY", this.ScrollY.ToString());
            AddAtrribute(graphNode, "ZoomScale", this.ZoomScale.ToString());

            //线路
            var linesNode = AddChildNode(graphNode, "Lines");
            foreach (var line in this.Graph.Lines)
            {
                var lineNode = AddChildNode(linesNode, "Line");
                AddAtrribute(lineNode, "Name", line.Name);
                AddAtrribute(lineNode, "Color", line.Color.ToArgb().ToString());
            }

            //站点
            var sitesNode = AddChildNode(graphNode, "Nodes");
            foreach (var site in this.Graph.Nodes)
            {
                var siteNode = AddChildNode(sitesNode, "Node");
                AddAtrribute(siteNode, "Name", site.Name);
                AddAtrribute(siteNode, "X", site.X.ToString());
                AddAtrribute(siteNode, "Y", site.Y.ToString());

                //路径
                foreach (var link in site.Links)
                {
                    var linkNode = AddChildNode(siteNode, "Link");
                    AddAtrribute(linkNode, "To", link.To.Name);
                    AddAtrribute(linkNode, "Line", link.Line.Name);
                    AddAtrribute(linkNode, "Flag", link.Flag.ToString());
                    AddAtrribute(linkNode, "Weight", link.Weight.ToString());
                }
            }

            doc.Save(fileName);
        }

        /// <summary>
        /// 从指定文件打开地铁线路图。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        public void OpenFromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            if (!System.IO.File.Exists(fileName)) return;

            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            var graphNode = doc.DocumentElement;
            this.ScrollX = int.Parse(graphNode.Attributes["ScrollX"].Value);
            this.ScrollY = int.Parse(graphNode.Attributes["ScrollY"].Value);
            this.ZoomScale = int.Parse(graphNode.Attributes["ZoomScale"].Value);

            //线路
            this.Graph.Lines.Clear();
            foreach (System.Xml.XmlNode lineNode in graphNode.SelectNodes("Lines/Line"))
            {
                this.Graph.Lines.Add(new MetroLine()
                {
                    Name = lineNode.Attributes["Name"].Value,
                    Color = Color.FromArgb(int.Parse(lineNode.Attributes["Color"].Value))
                });
            }

            //站点
            this.Graph.Nodes.Clear();
            foreach (System.Xml.XmlNode siteNode in graphNode.SelectNodes("Nodes/Node"))
            {
                this.Graph.Nodes.Add(new MetroNode()
                {
                    Name = siteNode.Attributes["Name"].Value,
                    X = int.Parse(siteNode.Attributes["X"].Value),
                    Y = int.Parse(siteNode.Attributes["Y"].Value)
                });
            }

            //路径
            foreach (System.Xml.XmlNode siteNode in graphNode.SelectNodes("Nodes/Node"))
            {
                var from = this.Graph.Nodes[siteNode.Attributes["Name"].Value];
                foreach (System.Xml.XmlNode linkNode in siteNode.SelectNodes("Link"))
                {
                    var to = this.Graph.Nodes[linkNode.Attributes["To"].Value];
                    var line = this.Graph.Lines[linkNode.Attributes["Line"].Value];
                    from.Links.Add(new MetroLink(line, from, to)
                    {
                        Flag = int.Parse(linkNode.Attributes["Flag"].Value),
                        Weight = float.Parse(linkNode.Attributes["Weight"].Value)
                    });
                }
            }

            Invalidate();
        }

        /// <summary>
        /// 查找乘车路线。
        /// </summary>
        /// <param name="startNodeName">起点的名称。</param>
        /// <param name="endNodeName">终点的名称。</param>
        public void FindPath(string startNodeName, string endNodeName)
        {
            if (!this.Graph.Nodes.Contains(startNodeName)) return;
            if (!this.Graph.Nodes.Contains(endNodeName)) return;

            this.StartNode = this.Graph.Nodes[startNodeName];
            this.EndNode = this.Graph.Nodes[endNodeName];
            this.CurPath = FindPath(this.StartNode, this.EndNode);

            Invalidate();
        }

        /// <summary>
        /// 查找乘车路线。
        /// </summary>
        /// <param name="startNode">起点。</param>
        /// <param name="endNode">终点。</param>
        /// <returns>乘车路线。</returns>
        public MetroPath FindPath(MetroNode startNode, MetroNode endNode)
        {
            MetroPath path = new MetroPath();
            if (startNode == null || endNode == null) return path;
            if (startNode == endNode) return path;

            //如果起点和终点拥有共同线路，则查找直达路线
            path = FindDirectPath(startNode, endNode);
            if (path.Links.Count > 0) return path;

            //如果起点和终点拥有一个共同的换乘站点，则查找一次换乘路线
            path = FindOneTransferPath(startNode, endNode);
            if (path.Links.Count > 0) return path;

            //查找路径最短的乘车路线
            var pathList = FindShortestPaths(startNode, endNode, null);

            //查找换乘次数最少的一条路线
            int minTransfers = int.MaxValue;
            foreach (var item in pathList)
            {
                var curTransfers = item.Transfers;
                if (curTransfers < minTransfers)
                {
                    minTransfers = curTransfers;
                    path = item;
                }
            }
            return path;
        }

        /// <summary>
        /// 查找直达路线。
        /// </summary>
        /// <param name="startNode">开始节点。</param>
        /// <param name="endNode">结束节点。</param>
        /// <returns>乘车路线。</returns>
        private MetroPath FindDirectPath(MetroNode startNode, MetroNode endNode)
        {
            MetroPath path = new MetroPath();

            var startLines = startNode.Links.Select(c => c.Line).Distinct().ToList();
            var endLines = endNode.Links.Select(c => c.Line).Distinct().ToList();

            var lines = startLines.Where(c => endLines.Contains(c)).ToList();
            if (lines.Count == 0) return path;

            //查找直达路线
            List<MetroPath> pathList = new List<MetroPath>();
            foreach (var line in lines)
            {
                pathList.AddRange(FindShortestPaths(startNode, endNode, line));
            }

            //挑选最短路线
            return GetShortestPath(pathList);
        }

        /// <summary>
        /// 查找一次中转的路线。
        /// </summary>
        /// <param name="startNode">开始节点。</param>
        /// <param name="endNode">结束节点。</param>
        /// <returns>乘车路线。</returns>
        private MetroPath FindOneTransferPath(MetroNode startNode, MetroNode endNode)
        {
            List<MetroPath> pathList = new List<MetroPath>();

            foreach (var startLine in startNode.Links.Select(c => c.Line).Distinct())
            {
                foreach (var endLine in endNode.Links.Select(c => c.Line).Where(c=> c != startLine).Distinct())
                {
                    //两条线路的中转站
                    foreach (var transferNode in this.Graph.GetTransferNodes(startLine, endLine))
                    {
                        //起点到中转站的直达路线
                        var startDirectPathList = FindShortestPaths(startNode, transferNode, startLine);

                        //中转站到终点的直达路线
                        var endDirectPathList = FindShortestPaths(transferNode, endNode, endLine);

                        //合并两条直达路线
                        foreach (var startDirectPath in startDirectPathList)
                        {
                            foreach (var endDirectPath in endDirectPathList)
                            {
                                var directPath = startDirectPath.Merge(endDirectPath);
                                pathList.Add(directPath);
                            }
                        }
                    }
                }
            }

            //挑选最短路线
            return GetShortestPath(pathList);
        }

        /// <summary>
        /// 从指定的路线列表中挑选一个最短的路线。
        /// </summary>
        /// <param name="pathList"></param>
        /// <returns></returns>
        private MetroPath GetShortestPath(List<MetroPath> pathList)
        {
            MetroPath minPath = new MetroPath();
            int minLength = int.MaxValue;
            foreach (var item in pathList)
            {
                if (item.Links.Count < minLength)
                {
                    minLength = item.Links.Count;
                    minPath = item;
                }
            }
            return minPath;
        }

        /// <summary>
        /// 查找指定两个节点之间的最短路径。
        /// </summary>
        /// <param name="startNode">开始节点。</param>
        /// <param name="endNode">结束节点。</param>
        /// <param name="line">目标线路（为null表示不限制线路）。</param>
        /// <returns>乘车路线列表。</returns>
        private List<MetroPath> FindShortestPaths(MetroNode startNode, MetroNode endNode, MetroLine line)
        {
            List<MetroPath> pathtList = new List<MetroPath>();
            if (startNode == endNode) return pathtList;

            //路径队列，用于遍历路径
            Queue<MetroPath> pathQueue = new Queue<MetroPath>();
            pathQueue.Enqueue(new MetroPath());

            while (pathQueue.Count > 0)
            {
                var path = pathQueue.Dequeue();

                //如果已经超过最短路径，则直接返回
                if (pathtList.Count > 0 && path.Links.Count > pathtList[0].Links.Count)
                    continue;

                //路径的最后一个节点
                MetroNode prevNode = path.Links.Count > 0 ? path.Links[path.Links.Count - 1].From : null;
                MetroNode lastNode = path.Links.Count > 0 ? path.Links[path.Links.Count - 1].To : startNode;

                //继续寻找后续节点
                foreach (var link in lastNode.Links.Where(c => c.To != prevNode && (line == null || c.Line == line)))
                {
                    if (link.To == endNode)
                    {
                        MetroPath newPath = path.Append(link);
                        if (pathtList.Count == 0 || newPath.Links.Count == pathtList[0].Links.Count)
                        {//找到一条路径
                            pathtList.Add(newPath);
                        }
                        else if (newPath.Links.Count < pathtList[0].Links.Count)
                        {//找到一条更短的路径
                            pathtList.Clear();
                            pathtList.Add(newPath);
                        }
                        else break;//更长的路径没有意义
                    }
                    else if (!path.ContainsNode(link.To))
                    {
                        pathQueue.Enqueue(path.Append(link));
                    }
                }
            }

            return pathtList;
        }

        /// <summary>
        /// 添加子节点。
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private XmlNode AddChildNode(XmlNode parentNode, string name)
        {
            var childNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, name, string.Empty);
            parentNode.AppendChild(childNode);
            return childNode;
        }

        /// <summary>
        /// 添加节点属性。
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private XmlAttribute AddAtrribute(XmlNode parentNode, string name, string value)
        {
            var attribute = parentNode.OwnerDocument.CreateAttribute(name);
            attribute.Value = value;
            parentNode.Attributes.Append(attribute);
            return attribute;
        }

        /// <summary>
        /// 获取指定位置处的站点。
        /// </summary>
        /// <param name="pt">目标位置。</param>
        /// <returns></returns>
        public MetroNode GetNodeAt(Point pt)
        {
            Point pt2 = ClientToMetro(pt);
            return this.Graph.Nodes.FirstOrDefault(c => GetNodeRect(c).Contains(pt2));
        }

        /// <summary>
        /// 将指定的坐标转换为地铁线路图坐标。
        /// </summary>
        /// <param name="pt">目标位置。</param>
        /// <returns></returns>
        public Point ClientToMetro(Point pt)
        {
            int x = (int)((pt.X - this.ScrollX) / this.ZoomScale);
            int y = (int)((pt.Y - this.ScrollY) / this.ZoomScale);
            return new Point(x, y);
        }

        /// <summary>
        /// 将指定的举行转换为地铁线路图坐标。
        /// </summary>
        /// <param name="rect">目标矩形。</param>
        /// <returns></returns>
        public Rectangle ClientToMetro(Rectangle rect)
        {
            Point pt = ClientToMetro(rect.Location);
            return new Rectangle(pt.X, pt.Y, (int)(rect.Width / this.ZoomScale), (int)(rect.Height / this.ZoomScale));
        }

        #endregion

        #region 绘图区域

        /// <summary>
        /// 重写OnPaint方法。
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //消除锯齿
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //滚动和缩放
            e.Graphics.TranslateTransform(this.ScrollX, this.ScrollY);
            e.Graphics.ScaleTransform(this.ZoomScale, this.ZoomScale);

            //绘制地铁线路图
            PaintGraph(e.Graphics, this.Graph);

            //绘制当前乘车路线
            PaintCurPath(e.Graphics);

            //绘制起点和终点
            PaintStartAndEndNodes(e.Graphics);

            //绘制线路列表
            PaintLineList(e.Graphics);
        }

        /// <summary>
        /// 绘制线路列表。
        /// </summary>
        /// <param name="g">绘图图面。</param>
        /// <param name="graph">地铁线路图。</param>
        private void PaintLineList(Graphics g)
        {
            if (this.Graph.Lines.Count == 0) return;

            g.ResetTransform();

            //遮罩层
            Rectangle rc = new Rectangle(10, 10, 150, (this.Graph.Lines.Count + 1) * 15);
            using (Brush brush = new SolidBrush(Color.FromArgb(180, Color.White)))
            {
                g.FillRectangle(brush, rc);
            }
            g.DrawRectangle(Pens.Gray, rc);

            //线路列表
            int y = rc.Y + 15;
            foreach (var line in this.Graph.Lines)
            {
                using (Pen pen = new Pen(line.Color, 5))
                {
                    g.DrawLine(pen, rc.X + 15, y, rc.X + 70, y);
                }

                var sz = g.MeasureString(line.Name, this.Font);
                g.DrawString(line.Name, this.Font, Brushes.Black, rc.X + 80, y - sz.Height / 2);

                y += 15;
            }
        }

        /// <summary>
        /// 绘制地铁线路图。
        /// </summary>
        /// <param name="g">绘图图面。</param>
        /// <param name="graph">地铁线路图。</param>
        private void PaintGraph(Graphics g, MetroGraph graph)
        {
            //绘制地铁路径
            foreach (var link in graph.Links.Where(c => c.Flag >= 0))
            {
                PaintLink(g, link);
            }

            //绘制地铁站点
            foreach (var node in graph.Nodes)
            {
                PaintNode(g, node);
            }
        }

        /// <summary>
        /// 绘制地铁站点。
        /// </summary>
        /// <param name="g">绘图图面。</param>
        /// <param name="node">地铁站点。</param>
        private void PaintNode(Graphics g, MetroNode node)
        {
            //绘制站点圆圈
            Color color = node.Links.Count > 2 ? Color.Black : node.Links[0].Line.Color;
            var rect = GetNodeRect(node);
            g.FillEllipse(Brushes.White, rect);
            using (Pen pen = new Pen(color))
            {
                g.DrawEllipse(pen, rect);
            }

            //绘制站点名称
            var sz = g.MeasureString(node.Name, this.Font).ToSize();
            Point pt = new Point(node.X - sz.Width / 2, node.Y + (rect.Height >> 1) + 4);
            g.DrawString(node.Name, Font, Brushes.Black, pt);
        }

        /// <summary>
        /// 绘制地铁站点间的线路。
        /// </summary>
        /// <param name="g">绘图图面。</param>
        /// <param name="link">地铁站点间的线路。</param>
        private void PaintLink(Graphics g, MetroLink link)
        {
            Point pt1 = new Point(link.From.X, link.From.Y);
            Point pt2 = new Point(link.To.X, link.To.Y);

            using (Pen pen = new Pen(link.Line.Color, 5))
            {
                pen.LineJoin = LineJoin.Round;
                if (link.Flag == 0)
                {//单线
                    g.DrawLine(pen, pt1, pt2);
                }
                else if (link.Flag > 0)
                {//双线并轨（如果是同向，则Flag分别为1和2，否则都为1）
                    float scale = (pen.Width / 2) / Distance(pt1, pt2);

                    float angle = (float)(Math.PI / 2);
                    if (link.Flag == 2) angle *= -1;

                    //平移线段
                    var pt3 = Rotate(pt2, pt1, angle, scale);
                    var pt4 = Rotate(pt1, pt2, -angle, scale);

                    g.DrawLine(pen, pt3, pt4);
                }
            }
        }

        /// <summary>
        /// 绘制起点和终点。
        /// </summary>
        /// <param name="g">绘图图面。</param>
        private void PaintStartAndEndNodes(Graphics g)
        {
            //绘制起点
            if (this.StartNode != null)
            {
                var startNodeImage = Properties.Resources.StartNode;
                int sx = this.StartNode.X - startNodeImage.Width / 2;
                int sy = this.StartNode.Y - startNodeImage.Height;
                g.DrawImage(startNodeImage, sx, sy);
            }

            //绘制终点
            if (this.EndNode != null)
            {
                var endNodeImage = Properties.Resources.EndNode;
                int ex = this.EndNode.X - endNodeImage.Width / 2;
                int ey = this.EndNode.Y - endNodeImage.Height;
                g.DrawImage(endNodeImage, ex, ey);
            }
        }

        /// <summary>
        /// 绘制当前乘车路线。
        /// </summary>
        /// <param name="g">绘图图面。</param>
        private void PaintCurPath(Graphics g)
        {
            if (this.CurPath.Links.Count == 0) return;

            //绘制白色遮罩层
            RectangleF rcMask = ClientToMetro(this.ClientRectangle);
            using (Brush brush = new SolidBrush(Color.FromArgb(200, Color.White)))
            {
                g.FillRectangle(brush, rcMask);
            }

            //绘制当前乘车路线
            foreach (var link in this.CurPath.Links)
            {
                //绘制路径
                if (link.Flag >= 0)
                {
                    PaintLink(g, link);
                }
                else
                {
                    //如果是隐藏的路径，则取反向的可见路径
                    var visibleLink = link.To.Links.FirstOrDefault(c => c.Flag >= 0 && c.To == link.From);
                    if (visibleLink != null)
                        PaintLink(g, visibleLink);
                }

                //绘制站点
                PaintNode(g, link.From);
                PaintNode(g, link.To);
            }
        }

        /// <summary>
        /// 获取地铁站点的矩形区域。
        /// </summary>
        /// <param name="node">地铁站点。</param>
        /// <returns></returns>
        private Rectangle GetNodeRect(MetroNode node)
        {
            int r = node.Links.Count > 2 ? 7 : 5;
            return new Rectangle(node.X - r, node.Y - r, (r << 1) + 1, (r << 1) + 1);
        }

        /// <summary>
        /// 矢量v以o为中心点，旋转angle角度，并缩放scale倍。
        /// </summary>
        /// <param name="v">要旋转的点。</param>
        /// <param name="o">中心点。</param>
        /// <param name="angle">旋转角度（以弧度为单位）。</param>
        /// <param name="scale">缩放比例。</param>
        /// <returns>旋转后的点。</returns>
        private Point Rotate(Point v, Point o, float angle, float scale)
        {
            v.X -= o.X;
            v.Y -= o.Y;
            double rx = scale * Math.Cos(angle);
            double ry = scale * Math.Sin(angle);
            double x = o.X + v.X * rx - v.Y * ry;
            double y = o.Y + v.X * ry + v.Y * rx;
            return new Point((int)x, (int)y);
        }

        /// <summary>
        /// 获取两点之间的距离。
        /// </summary>
        /// <param name="pt1">点1。</param>
        /// <param name="pt2">点2。</param>
        /// <returns>两点之间的距离。</returns>
        private float Distance(Point pt1, Point pt2)
        {
            return (float)Math.Sqrt((pt1.X - pt2.X) * (pt1.X - pt2.X) + (pt1.Y - pt2.Y) * (pt1.Y - pt2.Y));
        }

        #endregion

        #region 事件区域

        private Point m_mouseDownPoint = Point.Empty;
        private Point m_mouseLastPoint = Point.Empty;

        /// <summary>
        /// 重写OnMouseDown方法。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            m_mouseDownPoint = e.Location;
            m_mouseLastPoint = e.Location;
        }

        /// <summary>
        /// 重写OnMouseUp方法。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            //选择起点或终点
            var node = GetNodeAt(e.Location);
            if (node != null)
            {
                if (this.StartNode == null)
                {
                    this.StartNode = node;
                }
                else
                {
                    this.EndNode = node;

                    //查找乘车线路
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        m_curPath = FindPath(this.StartNode, this.EndNode);
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
            else if (Distance(e.Location, m_mouseDownPoint) < 3)//是否发生拖拽
            {
                this.StartNode = this.EndNode = null;
                this.CurPath.Links.Clear();
            }

            Invalidate();
        }

        /// <summary>
        /// 重写OnMouseMove方法。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.ScrollX += e.X - m_mouseLastPoint.X;
                this.ScrollY += e.Y - m_mouseLastPoint.Y;
                m_mouseLastPoint = e.Location;
            }
        }

        /// <summary>
        /// 重写OnMouseWheel方法。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            this.ZoomScale += (e.Delta > 0 ? 0.1f : -0.1f);
        }

        #endregion
    }
}