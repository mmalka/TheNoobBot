<?php
include "function_auth.php";

if (isset($_SERVER['PHP_AUTH_USER']) && mysql_real_escape_string($_SERVER['PHP_AUTH_USER']) != "" && isset($_SERVER['PHP_AUTH_PW']) && mysql_real_escape_string($_SERVER['PHP_AUTH_PW']) != "")
{
	if(isset($_GET['random']) && $_GET['random'])
	{
		header("retn: " . randomKeyValue(mysql_real_escape_string($_SERVER['PHP_AUTH_USER'])));
	}
	else if (isset($_GET['create']) && $_GET['create'])
	{
		header("retn: " . createSessionKey(mysql_real_escape_string($_SERVER['PHP_AUTH_USER']), mysql_real_escape_string($_SERVER['PHP_AUTH_PW'])));
		die;
	}
	else if (isset($_GET['TimeSubscription']) && $_GET['TimeSubscription'])
	{
		
		header("retn: " . secondeToStringDayHourMin(getEndTimeSubscription(mysql_real_escape_string($_SERVER['PHP_AUTH_USER']), mysql_real_escape_string($_SERVER['PHP_AUTH_PW'])) - time()));
		die;
	}
	else if(isset($_GET['transfer']) && $_GET['transfer'])
	{
		header("retn: " . getOldSubscription(mysql_real_escape_string($_SERVER['PHP_AUTH_USER']), mysql_real_escape_string($_SERVER['PHP_AUTH_PW'])));
	}
	else
	{
		header("retn: " . getSessionKey(mysql_real_escape_string($_SERVER['PHP_AUTH_USER']), mysql_real_escape_string($_SERVER['PHP_AUTH_PW'])));
		die;
	}
}

if (isset($_GET['botOnline']) && $_GET['botOnline'])
{
	echo botOnline();
	die;
}
?>