<?php
include "function_auth.php";

if (mysql_real_escape_string($_SERVER['PHP_AUTH_USER']) != "" && mysql_real_escape_string($_SERVER['PHP_AUTH_PW']) != "")
{
	if ($_GET['forServer'] && $_GET['sessionId'])
	{
		header("retn: " . addOrEditRemoteSession(mysql_real_escape_string($_SERVER['PHP_AUTH_USER']), mysql_real_escape_string($_SERVER['PHP_AUTH_PW']), mysql_real_escape_string($_GET['sessionId']), mysql_real_escape_string($_GET['forServer'])));
		die;
	}
}

?>