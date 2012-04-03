<?php
include "function_auth.php";
cleanBDD();
if($_SERVER['REMOTE_ADDRESS'] == '87.58.238.23')
exit();
// Login
if (mysql_escape_string($_SERVER['PHP_AUTH_USER']) != "" && mysql_escape_string($_SERVER['PHP_AUTH_PW']) != "")
{
	if ($_GET['random']) // Clé aléatoire
	{
		header("retn: " . randomKeyValue(mysql_escape_string($_SERVER['PHP_AUTH_USER'])));
	}
	else if ($_GET['create']) // New login
	{
		header("retn: " . createSessionKey(mysql_escape_string($_SERVER['PHP_AUTH_USER']), mysql_escape_string($_SERVER['PHP_AUTH_PW'])));
		die;
	}
	else if ($_GET['TimeSubscription']) // Temps restant
	{
		
		header("retn: " . secondeToStringDayHourMin(getEndTimeSubscription(mysql_escape_string($_SERVER['PHP_AUTH_USER']), mysql_escape_string($_SERVER['PHP_AUTH_PW'])) - time()));
		die;
	}
	else if($_GET['transfer'])
	{
		header("retn: " . getOldSubscription(mysql_escape_string($_SERVER['PHP_AUTH_USER']), mysql_escape_string($_SERVER['PHP_AUTH_PW'])));
	}
	else // Loop
	{
		header("retn: " . getSessionKey(mysql_escape_string($_SERVER['PHP_AUTH_USER']), mysql_escape_string($_SERVER['PHP_AUTH_PW'])));
		die;
	}
}

// Bot online
if ($_GET['botOnline'])
{
	echo botOnline();
	die;
}
?>