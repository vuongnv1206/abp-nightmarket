using System;
using System.Collections.Generic;
using System.Text;

namespace NightMarket.Admin.Systems.Users
{
	public class SetPasswordDto
	{
		public string NewPassword { get; set; }
		public string ConfirmNewPassword { get; set; }
	}
}
