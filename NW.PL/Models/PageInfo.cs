using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NW.PL.Models
{
    public class PageInfo
    {
        public string Controller { get; set; }
        public string View { get; set; }
        public string Title { get; set; }
        public string Layout { get; set; }

        private string ControllerHref => Controller + "/" + Controller;
        private string ViewHref => ControllerHref + View;
        public string LayoutHref => "~/Views/Layout/" + Layout + ".cshtml";

        public string ControllerStyleHref => "~/Resources/CSS/" + ControllerHref + ".css";
        public string ControllerStyleMinHref => "~/Resources/CSS/" + ControllerHref + ".min.css";
        public string ControllerStyleLessHref => "~/Resources/LESS/" + ControllerHref + ".less";

        public string ControllerScriptHref => "~/Resources/JS/" + ControllerHref + ".js";

        public string ViewStyleHref => "~/Resources/CSS/" + ViewHref + ".css";
        public string ViewStyleMinHref => "~/Resources/CSS/" + ViewHref + ".min.css";
        public string ViewStyleLessHref => "~/Resources/LESS/" + ViewHref + ".less";

        public string ViewScriptHref => "~/Resources/JS/" + ViewHref + ".js";

        public PageInfo(string Controller = null, string View = null)
        {
            this.Controller = Controller;
            this.Layout = Controller;
            this.View = View;
            this.Title = View;
        }
        
        public static PageInfo Create(string Controller = null, string View = null)
            => new PageInfo(Controller, View);

        public PageInfo setController(string Controller)
        {
            this.Controller = Controller;
            return this;
        }
        public PageInfo setView(string View)
        {
            this.View = View;
            return this;
        }
        public PageInfo setTitle(string Title)
        {
            this.Title = Title;
            return this;
        }
        public PageInfo setLayout(string Layout)
        {
            this.Layout = Layout;
            return this;
        }
    }
}