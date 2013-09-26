using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace WFActivityLibrary1
{
    public sealed class BookMarkActivity : NativeActivity<int>
    {
        [RequiredArgument]
        public InArgument<string> BookMarkName { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(NativeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            string name = context.GetValue(this.BookMarkName);

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException();
            }

            context.CreateBookmark(name, new BookmarkCallback(this.OnReadComplete));
        }

        void OnReadComplete(NativeActivityContext context, Bookmark bookmark, object state)
        {
            this.Result.Set(context, Convert.ToInt32(state));
        }

        protected override bool CanInduceIdle
        {
            get
            {
                return true;
            }
        }
    }
}
