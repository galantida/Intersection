using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace Game1
{
    public class clsColorReplacement
    {
        public Color original { get; }
        public Color replacement { get; }

        public clsColorReplacement(Color original, Color replacement)
        {
            this.original = original;
            this.replacement = replacement;
        }
    }
}
