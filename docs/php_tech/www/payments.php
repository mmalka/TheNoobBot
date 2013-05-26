<?php

include ("fusionchart.php");
$dbServer = "services.thenoobcompany.com";
$dbUser = "thenoobbot_chk";
$dbPassword = "XXXXXXXXXXXXXXXXX";
$mysql = mysql_connect($dbServer, $dbUser, $dbPassword) or die(mysql_error());
$type = $_GET['type'];
$db_site = "thenoobbot_site";
$payments_table = "wp_SWW_Invoice";

  /*
  //***** DATE GENERATOR *****
  $sql = "SELECT * FROM `$db_site`.`$payments_table`;";
  $query = mysql_query($sql);
  while($result = mysql_fetch_array($query))
  {
	 mysql_query("UPDATE `$db_site`.`$payments_table` SET `d` = '".gmdate("d", $result['date'])."', `m` =  '".gmdate("m", $result['date'])."', `Y` =  '".gmdate("Y", $result['date'])."' WHERE id = {$result['id']};");
  }
  die("fini");
  */
?>
<div style="font-size:12px;" class="post">
	<script language="Javascript" src="js/FusionCharts.js"></script>
	<h2 style="margin-left:15px;">TheNoobBot - Payments stats</h2>
		<div style="width:900px; height: 200px">
			<div style="float:left;width:300px; height: 200px">
				<h3 style="margin-left:15px;" id="stats">Approximate TheNoobBot revenues :</h3>
				<ul>
					<li><a href="payments.php?type=24months">Last twenty-four months</a></li>
					<li><a href="payments.php?type=12months">Last twelve months</a></li>
					<li><a href="payments.php?type=6months">Last six months</a></li>
					<li><a href="payments.php?type=180days">Last six months, days to days</a></li>
					<li><a href="payments.php?type=90days">Last three months, days to days</a></li>
                    <li><a href="payments.php?type=31days">Last thirty-one days</a></li>
					<li><a href="payments.php?type=15days">Last fifteen days</a></li>
					<li><a href="payments.php?type=7days">Last seven days</a></li>
				</ul>
			</div>
		</div>
<?php

switch ($type)
{
  case "15days":
    $count = 15;
    $value = 'day';
    $nom = "Last 15 days";
    break;
  case "31days":
    $count = 31;
    $value = 'day';
    $nom = "Last 31 days";
    break;
  case "90days":
    $count = 90;
    $value = 'day';
    $nom = "Last 90 days";
    break;
  case "180days":
    $count = 180;
    $value = 'day';
    $nom = "Last 180 days";
    break;
  case "6months":
    $count = 6;
    $value = 'month';
    $nom = "Last six months";
    break;
  case "12months":
    $count = 12;
    $value = 'month';
    $nom = "Last twelve months";
    break;
  case "24months":
    $count = 24;
    $value = 'month';
    $nom = "Last twenty-four months";
    break;
  case "7days":
  default:
    $count = 7;
    $value = 'day';
    $nom = 'Last 7 days';
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
      $requete = "`m` = '$m' AND `Y` = '$Y'";
      $thistime = date('m/Y', strtotime("-$y $value"));
      break;
    case "day":
      $thistime = date('d/m/Y', strtotime("-$y $value"));
      $d = date('d', strtotime("-$y $value"));
      $requete = "`m` = '$m' AND `d` = '$d' AND `Y` = '$Y'";
      break;
  }

  $arrData[$y][1] = $thistime;
  $query = mysql_query("SELECT count(id) as sales, SUM(price) as total FROM `$db_site`.`$payments_table` WHERE $requete ORDER by id DESC LIMIT 1;") or
	die(mysql_error());
  $result = mysql_fetch_array($query);
  $arrData[$y][3] = $result['sales'];
  $arrData[$y][2] = $result['total'];
  $strDataPrev = "<dataset seriesName='Sales'>";
  $strDataCurr = "<dataset seriesName='Revenue in Euros'>";
    
}
$strXML = "<chart showNames='1' showValues='0' rotateNames='1' animation='1' caption='$nom' numberPrefix='' formatNumberScale='1' rotateValues='30' placeValuesInside='10' decimals='0' >";
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