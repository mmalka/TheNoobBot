<?php
include "function_auth.php";
cleanBDD();
$userName = mysql_escape_string($_SERVER['PHP_AUTH_USER']);
$password = mysql_escape_string($_SERVER['PHP_AUTH_PW']);
if(getEndTimeSubscriptionPLATINIUM($userName, $password) >= time())
	echo 'PLATINIUM';
else
	echo $_SERVER['REMOTE_ADDR'];
?>