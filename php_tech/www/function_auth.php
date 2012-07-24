<?php
/*
List functions:

$mysql = connectionMysql();
closeMysql($mysql);
closeMysql();

$bool = existUserName($userName);
$userId = verifUserNameAndPassword($userName, $password);
$intTime = getEndTimeSubscription($userName, $password);

$sessionKey = getSessionKey($userName, $password);
$sessionKey = createSessionKey($userName, $password);

$numberOnlineBot = botOnline();

$hash = randomKeyValue($random);
$DHMS = secondeToStringDayHourMin($time);
*/


$dbServer = "localhost";
$dbName = "thenoobbot_site";
$dbUser = "thenoobbot_chk";
$dbPassword = "AFjuVhFmKXsQ2wAj";

$tableUsersName = "wp_users";
$tableSubscription = "wp_SWW_Subscription";
$tableCurrentConnection = "wp_SWW_Current_Connection";
$tableRemote = "wp_SWW_Remote";
$tableStats = "wp_SWW_Stats";

$secret = 'vsdGDFDSFF4578sGDSD65csds89df4sfcds65vF';
 
$mysql = NULL;
 
function connectionMysql()
{
	global $dbServe, $dbUser, $dbPassword, $dbName, $mysql;
	$mysql = mysql_connect($dbServer,$dbUser,$dbPassword);
	if (!$mysql)
	  die ("Connection error");
	mysql_select_db($dbName) or die ("Connection error");
	return $mysql;
}
function closeMysql()
{
	global $mysql;
	if ($mysql != NULL)
		mysql_close($mysql);
	$mysql = NULL;
}

function existUserName($userName)
{
	global $tableUsersName;
	connectionMysql();
	$requete="SELECT * FROM $tableUsersName WHERE user_login = '$userName';";
	$query = mysql_query($requete) or die(mysql_error());
	$row = mysql_fetch_array($query);
	closeMysql();
	
	if ($row)
		return true;
	return false;
}
 
function verifUserNameAndPassword($userName, $password)
{
	global $tableUsersName;
	connectionMysql();
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
			closeMysql();
			return $idUser;
		}
	}
	closeMysql();
	return -1;
}
function getEndTimeSubscription($userName, $password)
{
	global $tableSubscription;
	$userId = verifUserNameAndPassword($userName, $password);
	if ($userId > 0)
	{
		connectionMysql();
		$query = mysql_query("SELECT * FROM $tableSubscription 
                              WHERE idMember = $userId ") or die(mysql_error());
		$row = mysql_fetch_array($query);
		if ($row)
		{
			$result = $row['endDate'];
			closeMysql();
			return $result;
		}
		
	}
	closeMysql();
	return 0;
}

function getSessionKey($userName, $password)
{
	global $tableCurrentConnection, $tableStats;
	$idUser = verifUserNameAndPassword($userName, $password);
	if ($idUser > 0 && getEndTimeSubscription($userName, $password) >= time())
	{
		connectionMysql();
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
            $honnor = intval($_GET['honnor']);
            if($honnor > 4000 or $honnor < 0)
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
			closeMysql();			
			return $row['sessionKey'];
		}
		closeMysql();
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
			$sessionKey = md5($secret . $_SERVER['REMOTE_ADDR'] . $userName);
			connectionMysql();
			mysql_query("DELETE FROM $tableCurrentConnection WHERE idUser=$idUser");
			mysql_query("INSERT INTO $tableCurrentConnection VALUES(NULL, '$sessionKey', $idUser, ".time().")");
            $FirstConnexionCheck = mysql_query("SELECT idUser FROM $tableStats WHERE idUser=$idUser;");
            if(!mysql_num_rows($FirstConnexionCheck))
              mysql_query("INSERT INTO $tableStats VALUES($idUser, 0, 0, 0, 0, 0, 0)");
			closeMysql();
			return $sessionKey;
		}
		else
		{
			connectionMysql();
			mysql_query("DELETE FROM $tableCurrentConnection WHERE idUser=$idUser");
			mysql_query("INSERT INTO $tableCurrentConnection VALUES(NULL, 'trial', $idUser, ".(time()+(20*60)).")");
			closeMysql();
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
	global $tableCurrentConnection;
	connectionMysql();
	$query = mysql_query("SELECT id FROM $tableCurrentConnection 
                              WHERE lastTime > ".(time()-160)) or die(mysql_error());
	$n = mysql_num_rows($query);
	closeMysql();
	return $n;
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
		connectionMysql();
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
		closeMysql();
	}
	return $ret;
}

function cleanBDD()
{
	global $tableRemote, $tableCurrentConnection;
	connectionMysql();
	mysql_query("DELETE FROM $tableCurrentConnection WHERE lastTime < ".(time()-(3600*24))."") or die(mysql_error());
	mysql_query("DELETE FROM $tableRemote WHERE lastTime < ".(time()-13)."") or die(mysql_error());
	closeMysql();
}

function getOldSubscription($email, $password)
{
	$tableemail = "tblclients";
	$Product = "tblhosting";
	$Orders = "tblorders";
	$Invoices = "tblinvoices";
	
	connectionMysql();
	
	$reponse = mysql_query("SELECT * FROM $tableemail WHERE email = '$email'");
	while ($donnees = mysql_fetch_array($reponse) )
	{
		if ($donnees['transfered'] != "0")
		{
			closeMysql();
			return "";
			die;
		}
		// Crypte password
		$saltArray = explode(":", $donnees['password']);
		$salt = $saltArray[1];
		$passwordCrypted = md5($salt . $password) . ":" . $salt;

		if ($donnees['password'] == $passwordCrypted)
		{ // If password good
			$reponse2 = mysql_query("SELECT *
							FROM `" . $Product . "` WHERE userid = '".$donnees['id']."'");
			while ($donnees2 = mysql_fetch_array($reponse2) )
			{
				if ($donnees2['domainstatus'] == "Active" or  $donnees2['domainstatus'] == "Pending")
				{
					// By Date
					    list($year, $month, $day) = explode('-', $donnees2['nextduedate']);
						$timestamp = mktime(0, 0, 0, $month, $day, $year);
					if ($timestamp > time())
					{
						closeMysql();
						return "true";
						die;
					}
					
					// For Life time
					if ($donnees2['billingcycle'] == "One Time")
					{
						$reponse3 = mysql_query("SELECT *
							FROM `" . $Orders . "` WHERE id = '".$donnees2['orderid']."'");
						while ($donnees3 = mysql_fetch_array($reponse3))
						{
							if ($donnees3['invoiceid'] > 0)
							{
								$reponse4 = mysql_query("SELECT *
									FROM `" . $Invoices . "` WHERE id = '".$donnees3['invoiceid']."'");
								while ($donnees4 = mysql_fetch_array($reponse4))
								{
									if ($donnees4['status'] == "Paid")
									{
										closeMysql();
										return "true";
										die;
									}
								}
							}
						}
					}
				}
			}
		}
	}
closeMysql();
return "";
}
?>