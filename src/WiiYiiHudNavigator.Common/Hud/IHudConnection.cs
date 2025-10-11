using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiiYiiHudNavigator.Common.Models;

namespace WiiYiiHudNavigator.Common.Hud;

public interface IHudConnection
{
	void NoNavigationData();

	Task UpdateNavigationData(HudNavigationData navigationData, bool hasNoChange = false);
}
