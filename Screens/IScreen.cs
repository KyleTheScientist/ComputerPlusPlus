using GorillaNetworking;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerPlusPlus
{
    public interface IScreen
    {
        public string Title { get; }
        public string Description { get; }

        public string GetContent();
        public void OnKeyPressed(GorillaKeyboardButton button);
        public void Start();
    }
}
