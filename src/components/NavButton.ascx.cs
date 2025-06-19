using System;
using System.Web.UI;

namespace CodingRep.components
{
    public partial class NavButton : UserControl
    {
        public string iconUrl
        {
            get => imgNavIcon.Src;
            set => imgNavIcon.Src = value;
        }

        public bool describeDisplay { get; set; } = true;

        public string navigateUrl
        {
            get => hyLink.NavigateUrl;
            set => hyLink.NavigateUrl = value;
        }
        
        public string text { get; set; } = "default";
        public string width
        {
            get => imgNavIcon.Style["width"];
            set => imgNavIcon.Style["width"] = value;
        }

        public string height
        {
            get => imgNavIcon.Style["height"];
            set => imgNavIcon.Style["height"] = value;
        }

        public bool isOpposition { get; set; } = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (describeDisplay)
                {
                    contentBind();
                }

                if (isOpposition)
                {
                    imageOpposition();
                }
            }
        }

        private void contentBind()
        {
            dynamicCss.Text = @"<style>.nav-button:hover::after {
            content: '" + text + @"';
            position: absolute;
            bottom: -40px; /* 解释文本的位置 */
            left: 50%;
            transform: translateX(-50%);
            background-color: gray;
            border: 1px solid #ccc;
            color: white;
            min-width: 50px; /* 设置最小宽度 */
            padding: 5px;
            z-index: 1;
            border-radius: 5px;
            text-align: center; /* 文本居中显示 */}
            </style>";
        }

        private void imageOpposition()
        {
            Opposition.Text =
                @"<style>.nav-icon{filter: invert(1);}";
        }
    }
}

