<?php
include "function_auth.php";
cleanBDD();
// Login
//isset($_SERVER['PHP_AUTH_USER']) && 
//&& isset($_SERVER['PHP_AUTH_PW']) 
if (isset($_SERVER['PHP_AUTH_USER']) && mysql_escape_string($_SERVER['PHP_AUTH_USER']) != "" && isset($_SERVER['PHP_AUTH_PW']) && mysql_escape_string($_SERVER['PHP_AUTH_PW']) != "")
{
	if(isset($_GET['random']) && $_GET['random'])// Clé aléatoire
	{
		header("retn: " . randomKeyValue(mysql_escape_string($_SERVER['PHP_AUTH_USER'])));
	}
	else if (isset($_GET['create']) && $_GET['create']) // New login
	{
		header("retn: " . createSessionKey(mysql_escape_string($_SERVER['PHP_AUTH_USER']), mysql_escape_string($_SERVER['PHP_AUTH_PW'])));
		die;
	}
	else if (isset($_GET['TimeSubscription']) && $_GET['TimeSubscription']) // Temps restant
	{
		
		header("retn: " . secondeToStringDayHourMin(getEndTimeSubscription(mysql_escape_string($_SERVER['PHP_AUTH_USER']), mysql_escape_string($_SERVER['PHP_AUTH_PW'])) - time()));
		die;
	}
	else if(isset($_GET['transfer']) && $_GET['transfer'])
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
if (isset($_GET['botOnline']) && $_GET['botOnline'])
{
	echo botOnline();
	die;
}
?>