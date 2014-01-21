using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELMAH_Viewer
{
	public class ElmahViewerException : Exception { }

	public class InvalidValueException : ElmahViewerException { }
}
