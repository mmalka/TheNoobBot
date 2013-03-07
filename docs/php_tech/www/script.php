<?php

$R = htmlspecialchars($_GET["r"]);
$IP = preg_replace("/[^0-9]/", "", $_SERVER["REMOTE_ADDR"]);

$page = "";
$keyDecrypt = 0;


$page=file_get_contents("Protected/script54cdDFCE45c.cs"); 

for ($i=0;$i<strlen($IP);$i++) 
{
    $keyDecrypt += $IP{$i};
}


for ($i=0;$i<strlen($page);$i++) 
{
	echo ord($page{$i}) + $keyDecrypt + ($R*2);
    echo " ";
}


?>