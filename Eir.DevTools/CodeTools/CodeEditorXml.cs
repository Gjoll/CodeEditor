using System;
using System.Collections.Generic;
using System.Text;

namespace Eir.DevTools
{
    public class CodeEditorXml : CodeEditor
    {
        public CodeEditorXml()
        {
            this.BlockStart = "<!-- +";
            this.BlockStartTerm = "-->";
            this.BlockEnd = "<!-- -";
            this.BlockEndTerm = "-->";
        }
    }
}
