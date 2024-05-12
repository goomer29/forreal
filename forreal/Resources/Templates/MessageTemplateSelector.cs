using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using forreal.Models;

namespace forreal.Resources.Templates
{
    public class MessageTemplateSelector: DataTemplateSelector
    {
        public DataTemplate CurrentUserTemplate { get; set; }
        public DataTemplate OtherUserTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var message = item as Chat; 
            if (message == null)
                return null;

            return message.IsCurrentUser ? CurrentUserTemplate : OtherUserTemplate;
        }
    }
}
