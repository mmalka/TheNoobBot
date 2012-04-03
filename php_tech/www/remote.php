<?php
include "function_auth.php";
cleanBDD();
// Login
if (mysql_escape_string($_SERVER['PHP_AUTH_USER']) != "" && mysql_escape_string($_SERVER['PHP_AUTH_PW']) != "")
{
	if ($_GET['forServer'] && $_GET['sessionId']) // New login
	{
		header("retn: " . addOrEditRemoteSession(mysql_escape_string($_SERVER['PHP_AUTH_USER']), mysql_escape_string($_SERVER['PHP_AUTH_PW']), mysql_escape_string($_GET['sessionId']), mysql_escape_string($_GET['forServer'])));
		die;
	}
}

?>