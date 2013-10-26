<?php

include ("fusionchart.php");
$dbServer = "localhost";
$dbUser = "thenoobbot_chk";
$dbPassword = "XXXXXXXXXXXXXXXXX";
$mysql = mysql_connect($dbServer, $dbUser, $dbPassword) or die(mysql_error());
$type = $_GET['type'];
$db_site = "thenoobbot_site";
$stats_table = "tnb_stats";

?>
<div style="font-size:12px;" class="post">
	<script language="Javascript" src="js/FusionCharts.js"></script>
	<h2 style="margin-left:15px;">TheNoobBot Online bots stats</h2>
		<div style="width:900px; height: 200px">
			<div style="float:left;width:300px; height: 200px">
								<h3 style="margin-left:15px;" id="stats">Global online bots stats :</h3>
				<ul>
					<li><a href=".advanced.php?type=month">Last twelve months</a></li>
					<li><a href=".advanced.php?type=days">Last thirty-one days</a></li>
					<li><a href=".advanced.php?type=day">Last seven days</a></li>
					<li><a href=".advanced.php?type=sdays">Last seven days, hour per hour</a></li>
					<li><a href=".advanced.php?type=hour">Last 24 hours</a></li>
					<li><a href=".advanced.php?type=12h">Last 12 hours, minutes per minutes (slow!)</a></li>
					<li><a href=".advanced.php?type=24h">Last 24 hours, minutes per minutes (slow!)</a></li>
					<li><a href=".advanced.php?type=mins">Last hour, minutes per minutes</a></li>
				</ul>
			</div>
			<div style="float:left;width:300px; height: 200px">
				<h3 style="margin-left:15px;" id="stats">Paid online bots stats :</h3>
				<ul>
					<li><a href=".advanced.php?type=month&info=paid">Last twelve months</a></li>
					<li><a href=".advanced.php?type=days&info=paid">Last thirty-one days</a></li>
					<li><a href=".advanced.php?type=day&info=paid">Last seven days</a></li>
					<li><a href=".advanced.php?type=sdays&info=paid">Last seven days, hour per hour</a></li>
					<li><a href=".advanced.php?type=hour&info=paid">Last 24 hours</a></li>
					<li><a href=".advanced.php?type=12h&info=paid">Last 12 hours, minutes per minutes (slow!)</a></li>
					<li><a href=".advanced.php?type=24h&info=paid">Last 24 hours, minutes per minutes (slow!)</a></li>
					<li><a href=".advanced.php?type=mins&info=paid">Last hour, minutes per minutes</a></li>
				</ul>
			</div>
			<div style="float:left;width:300px; height: 200px">
				<h3 style="margin-left:15px;" id="stats">Free trial online bots stats :</h3>
				<ul>
					<li><a href=".advanced.php?type=month&info=trial">Last twelve months</a></li>
					<li><a href=".advanced.php?type=days&info=trial">Last thirty-one days</a></li>
					<li><a href=".advanced.php?type=day&info=trial">Last seven days</a></li>
					<li><a href=".advanced.php?type=sdays&info=trial">Last seven days, hour per hour</a></li>
					<li><a href=".advanced.php?type=hour&info=trial">Last 24 hours</a></li>
					<li><a href=".advanced.php?type=12h&info=trial">Last 12 hours, minutes per minutes (slow!)</a></li>
					<li><a href=".advanced.php?type=24h&info=trial">Last 24 hours, minutes per minutes (slow!)</a></li>
					<li><a href=".advanced.php?type=mins&info=trial">Last hour, minutes per minutes</a></li>
				</ul>
			</div>
		</div>
<?php

switch ($type)
{
  case "days": // jour - mois 28/29/30/31
    $count = 31;
    $value = 'day';
    $nom = "Last thirty-one days";
    break;
  case "month": // mois - année 12
    $count = 12;
    $value = 'month';
    $nom = "Last twelve months";
    break;
  case "hour": // semaine - mois 4
    $count = 24;
    $value = 'hour';
    $nom = 'Last 24 hours';
    break;
  case "mins": // semaine - mois 4
    $count = 60;
    $value = 'minutes';
    $nom = 'Last hour, minutes per minutes';
    break;
  case "12h": // semaine - mois 4
    $count = 144;
    $value = 'minutes';
    $nom = 'Last 12 hours, minutes per minutes (slow!)';
    break;
  case "24h": // semaine - mois 4
    $count = 288;
    $value = 'minutes';
    $nom = 'Last 24 hours, minutes per minutes (slow!)';
    break;
  case "sdays":
    $count = 189;
    $value = 'hour';
    $nom = 'Last seven days, hour per hour';
    break;
  case "day":
  default:
    $count = 7;
    $value = 'day';
    $nom = 'Last seven days';
    break;
}
for ($i = $count; $i > 0; $i--)
{
  $y = $i - 1;
  $Y = date('Y', strtotime("-$y $value"));
  $m = date('m', strtotime("-$y $value"));
  switch ($value)
  {
    case "month":
      $requete = "`m` = '$m'";
      $thistime = date('m/Y', strtotime("-$y $value"));
      break;
    case "hour":
      if ($type == "sdays")
      {
        $H = date('H', strtotime("-$y $value"));
        $d = date('d', strtotime("-$y $value"));
        $requete = "`m` = '$m' AND `d` = '$d' AND `H` = '$H'";
        $thistime = date('H\h', strtotime("-$y $value"));
        if ($H == 0 or $H == 12 or $H == 6 or $H == 18)
        {
          if ($H == 0)
          {
            $thistime = date('\l\e d/m \à \m\i\n\u\i\t', strtotime("-$y $value"));
          }
        }
        else
        {
          $thistime = ' ';
        }
      }
      else
      {
        $H = date('H', strtotime("-$y $value"));
        $d = date('d', strtotime("-$y $value"));
        $requete = "`m` = '$m' AND `d` = '$d' AND `H` = '$H'";
        $thistime = date('H\h', strtotime("-$y $value"));
      }
      break;
    case "minutes":
      if ($type == '24h' or $type == '12h')
      {
        $x = (5 * $y) - 1;
      }
      else
      {
        $x = $y + 1;
      }
      $H = date('H', strtotime("-$x $value"));
      $d = date('d', strtotime("-$x $value"));
      $i2 = date('i', strtotime("-$x $value"));
      $requete = "`m` = '$m' AND `d` = '$d' AND `H` = '$H' AND `i` = '$i2'";
      $thistime = date('H', strtotime("-$x $value"));
      $thistime .= 'h';
      $thistime .= date('i', strtotime("-$x $value"));
      $thistime .= 'mins';
      if ($type == '12h' or $type == '24h')
      {
        if ($i2 == 5 or $i2 == 6 or $i2 == 7 or $i2 == 8 or $i2 == 9)
        {
          $thistime = date('H', strtotime("-$x $value"));
          $thistime .= 'h';
        }
        else
        {
          $thistime = ' ';
        }
      }
      break;
    case "day":
      $thistime = date('d/m', strtotime("-$y $value"));
      $d = date('d', strtotime("-$y $value"));
      $requete = "`m` = '$m' AND `d` = '$d'";
      break;
  }

  $arrData[$y][1] = $thistime;
  if ($_GET['info'] == 'paid')
  {
    $query = mysql_query("SELECT online_paid, multiplicator FROM `$db_site`.`$stats_table` WHERE $requete ORDER by online_paid DESC LIMIT 1;") or
      die(mysql_error());
    $result = mysql_fetch_array($query);
    $online = $result['online_paid'];
    if ($online > 0) $arrData[$y][2] = intval($online * $result['multiplicator']);
    else  $arrData[$y][2] = $online;
    $arrData[$y][3] = $online;
    $strDataPrev = "<dataset seriesName='Real Paid Online Bots'>";
    $strDataCurr = "<dataset seriesName='Shown Paid Online Bots'>";
  }
  else
    if ($_GET['info'] == 'trial')
    {
      $query = mysql_query("SELECT online_trial, multiplicator FROM `$db_site`.`$stats_table` WHERE $requete ORDER by online_trial DESC LIMIT 1;") or
        die(mysql_error());
      $result = mysql_fetch_array($query);
      $online = $result['online_trial'];
      if ($online > 0) $arrData[$y][2] = intval($online * $result['multiplicator']);
      else  $arrData[$y][2] = $online;
      $arrData[$y][3] = $online;
      $strDataPrev = "<dataset seriesName='Real Trial Online Bots'>";
      $strDataCurr = "<dataset seriesName='Shown Trial Online Bots'>";
    }
    else
    {
      $query = mysql_query("SELECT online_bots, multiplicator FROM `$db_site`.`$stats_table` WHERE $requete ORDER by online_bots DESC LIMIT 1;") or
        die(mysql_error());
      $result = mysql_fetch_array($query);
      $online = $result['online_bots'];
      if ($online > 0) $arrData[$y][2] = intval($online * $result['multiplicator']);
      else  $arrData[$y][2] = $online;
      $arrData[$y][3] = $online;
      $strDataPrev = "<dataset seriesName='Real Global Online Bots'>";
      $strDataCurr = "<dataset seriesName='Shown Global Online Bots'>";
    }
}
$strXML = "<chart showNames='1' showValues='0' rotateNames='1' animation='1' caption='$nom' numberPrefix='' formatNumberScale='0' rotateValues='30' placeValuesInside='10' decimals='0' >";
$strCategories = "<categories>";


foreach ($arrData as $arSubData)
{
  $strCategories .= "<category name='".$arSubData[1]."' />";
  $strDataCurr .= "<set value='".$arSubData[2]."' />";
  $strDataPrev .= "<set value='".$arSubData[3]."' />";
}
$strCategories .= "</categories>";
$strDataCurr .= "</dataset>";
$strDataPrev .= "</dataset>";
$strXML .= $strCategories.$strDataCurr.$strDataPrev."</chart>";
echo '<div style="background:#fff;margin:0 auto;height:500px;width:620px;">';
echo renderChart("flash/MSLine.swf", "", $strXML, "", 620, 500, false, false);
echo '</div>';

?>
</div>