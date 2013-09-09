<?php
if (!isset($_SERVER["HTTP_USER_AGENT"]) || $_SERVER["HTTP_USER_AGENT"] == "")
	exit (0);
if (isset($_SERVER["HTTP_CF_CONNECTING_IP"]))
	$_SERVER["REMOTE_ADDR"] = $_SERVER["HTTP_CF_CONNECTING_IP"];
$dbServer = "services.thenoobcompany.com";
$dbName = "thenoobbot_site";
$dbUser = "thenoobbot_chk";
$dbPassword = "XXXXXXXXXXXXXXXXX";

$tableUsersName = "wp_users";
$tableSubscription = "wp_SWW_Subscription";
$tableSubscriptionPLATINIUM = "wp_SWW_SubscriptionPLATINIUM";
$tableCurrentConnection = "wp_SWW_Current_Connection";
$tableRemote = "wp_SWW_Remote";
$tableStats = "wp_SWW_Stats";

$secret = '0e8897c8c73772e72d81dd28ebf57ac3';
 
$mysql = NULL;
 
function connectionMysql()
{
	global $dbServer, $dbUser, $dbPassword, $dbName, $mysql;
	$mysql = mysql_connect($dbServer,$dbUser,$dbPassword) or die(mysql_error());
	if (!$mysql)
	  die ("Connection error");
	mysql_select_db($dbName) or die ("Connection error");
	return $mysql;
}
connectionMysql();
function existUserName($userName)
{
	global $tableUsersName;
	
	$requete="SELECT * FROM $tableUsersName WHERE user_login = '$userName';";
	$query = mysql_query($requete) or die(mysql_error());
	$row = mysql_fetch_array($query);
	
	
	if ($row)
		return true;
	return false;
}
 
function verifUserNameAndPassword($userName, $password)
{
	global $tableUsersName;
	
	$query = mysql_query("SELECT * FROM $tableUsersName 
                              WHERE user_login = '$userName'") or die(mysql_error());
	$row = mysql_fetch_array($query);
	if ($row)
	{
		require_once('class-phpass.php');
		$wp_hasher = new PasswordHash(8, TRUE);
	 
		$password_hashed = $row['user_pass'];
		$idUser = $row['ID'];
		if($wp_hasher->CheckPassword($password, $password_hashed)
			   || $password_hashed == md5($password)) {
			
			return $idUser;
		}
	}
	
	return -1;
}
function getEndTimeSubscription($userName, $password)
{
	global $tableSubscription;
	$userId = verifUserNameAndPassword($userName, $password);
	if ($userId > 0)
	{
		
		$query = mysql_query("SELECT * FROM $tableSubscription 
                              WHERE idMember = $userId ") or die(mysql_error());
		$row = mysql_fetch_array($query);
		if ($row)
		{
			$result = $row['endDate'];
			
			return $result;
		}
		
	}
	
	return 0;
}
function getEndTimeSubscriptionPLATINIUM($userName, $password)
{
	global $tableSubscriptionPLATINIUM;
	$userId = verifUserNameAndPassword($userName, $password);
	if ($userId > 0)
	{
		
		$query = mysql_query("SELECT * FROM $tableSubscriptionPLATINIUM 
                              WHERE idMember = $userId ") or die(mysql_error());
		$row = mysql_fetch_array($query);
		if ($row)
		{
			$result = $row['endDate'];
			
			return $result;
		}
		
	}
	
	return 0;
}

function getSessionKey($userName, $password)
{
	global $tableCurrentConnection, $tableStats;
	$idUser = verifUserNameAndPassword($userName, $password);
	if ($idUser > 0 && getEndTimeSubscription($userName, $password) >= time())
	{
		
		$query = mysql_query("SELECT * FROM $tableCurrentConnection 
								  WHERE idUser = $idUser ") or die(mysql_error());
		$row = mysql_fetch_array($query);
		if ($row)
		{
			mysql_query("UPDATE $tableCurrentConnection  SET lastTime = ".time()." WHERE idUser = $idUser");
            $onlineTime = time()-$row['lastTime'];
            if($onlineTime > 180)
              $onlineTime = 55;
            $level = intval($_GET['level']);
            if($level > 3)
              $level = 1;
			  if (isset($_GET['honnor']))
			  {
				$honnor = intval($_GET['honnor']);
				if($honnor > 4000 or $honnor < 0)
				  $honnor = 0;
			  }
			  else
				$honnor = 0;
            $exp = intval($_GET['exp']);
            if($exp > 10000000 or $exp < 0)
              $exp = 0;
            $farm = intval($_GET['farm']);
            if($farm > 1000 or $farm < 0)
              $farm = 0;
            $kill = intval($_GET['kill']);
            if($kill > 1000 or $kill < 0)
              $kill = 0;
            $sql = "UPDATE `$tableStats` SET `onlineTime` = `onlineTime`+$onlineTime, `level` = `level`+$level, `honnor` = honnor+$honnor, `exp` = `exp`+$exp, `farm` = `farm`+$farm, `kill` = `kill`+$kill WHERE idUser = $idUser;";
            mysql_query($sql);
						
			return $row['sessionKey'];
		}
		
	}
    else
      echo "SNVConnect";
	return "";
}
function createSessionKey($userName, $password)
{
	global $tableCurrentConnection, $tableStats, $secret;
	$idUser = verifUserNameAndPassword($userName, $password);
	if ($idUser > 0)
	{
		if (getEndTimeSubscription($userName, $password) >= time())
		{
			if(getEndTimeSubscriptionPLATINIUM($userName, $password) >= time())
				$sessionKey = md5($secret . "PLATINIUM" . $userName);
			else
				$sessionKey = md5($secret . $_SERVER['REMOTE_ADDR'] . $userName);
			
			mysql_query("DELETE FROM $tableCurrentConnection WHERE idUser=$idUser");
			mysql_query("INSERT INTO $tableCurrentConnection VALUES(NULL, '$sessionKey', $idUser, ".time().")");
            $FirstConnexionCheck = mysql_query("SELECT idUser FROM $tableStats WHERE idUser=$idUser;");
            if(!mysql_num_rows($FirstConnexionCheck))
              mysql_query("INSERT INTO $tableStats VALUES($idUser, 0, 0, 0, 0, 0, 0)");
			
			return $sessionKey;
		}
		else
		{
			
			mysql_query("DELETE FROM $tableCurrentConnection WHERE idUser=$idUser");
			mysql_query("INSERT INTO $tableCurrentConnection VALUES(NULL, 'trial', $idUser, ".(time()+(20*60)).")");
			
			echo "SNVConnect";
		}
	}
	else
	{
		if(!existUserName($userName))
			echo "LEConnect";
		else
			echo "PEConnect";
	}
	return "";
}

function botOnline()
{
	$query = mysql_query("SELECT online_bots, multiplicator FROM `tnb_stats` ORDER by id DESC LIMIT 1") or die(mysql_error());
	$result = mysql_fetch_assoc($query);
	return intval($result["online_bots"]*$result["multiplicator"]);
}

function randomKeyValue($random)
{
	global $secret;
	return md5(($random*4) . $secret);
}
// Convertire seconde en Jours - Heure - Minute - Seconde
function secondeToStringDayHourMin($time)
{
	if ($time>=86400)
	{
		$jour = floor($time/86400);
		$reste = $time%86400;

		$heure = floor($reste/3600);
		$reste = $reste%3600;

		$minute = floor($reste/60);

		$seconde = $reste%60;

		$result = $jour.'days '.$heure.'h '.$minute.'min '.$seconde.'s';
	}
	elseif ($time < 86400 AND $time>=3600)
	{
		$heure = floor($time/3600);
		$reste = $time%3600;

		$minute = floor($reste/60);

		$seconde = $reste%60;

		$result = $heure.'h '.$minute.'min '.$seconde.' s';
	}
	elseif ($time<3600 AND $time>=60)
	{
		$minute = floor($time/60);
		$seconde = $time%60;
		$result = $minute.'min '.$seconde.'s';
	}
	elseif ($time < 60)
	{
		$result = $time.'s';
	}
	return $result;
}

function addOrEditRemoteSession($userName, $password, $sessionKey, $forServer)
{
	global $tableRemote;
	$ret = "";
	$idUser = verifUserNameAndPassword($userName, $password);
	if ($idUser > 0 && getEndTimeSubscription($userName, $password) >= time())
	{
		
		$query = mysql_query("SELECT * FROM $tableRemote 
								  WHERE ( idUser = $idUser AND sessionKey = $sessionKey )") or die(mysql_error());
		$row = mysql_fetch_array($query);
		if ($row)
		{
			$ret = $row['forBot'];
			mysql_query("UPDATE $tableRemote  SET lastTime = ".time().", forServer = '$forServer', forBot = '' WHERE idUser = $idUser AND sessionKey = $sessionKey") or die(mysql_error());		
		}
		else
		{
			mysql_query("INSERT INTO $tableRemote VALUES(NULL, $idUser, $sessionKey, '$forServer', '', ".time().")") or die(mysql_error());
		}
		
	}
	return $ret;
}
?>