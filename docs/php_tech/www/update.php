<?php
	$currentrev = "1.9.9";
	$wow_patch = "5.4.1";
	$forumlink = "http://thenoobbot.com/community/viewtopic.php?f=8&t=6418";
	$month_release = "11";
	$day_release = "21";
	$year_release = "2013";
 if (!isset($forwebsite) || !$forwebsite) 
 {
    if (isset($_GET['show']) && $_GET['show'] == 'desc')
	  echo '1.9.9 for WoW 5.4.1';
    elseif (isset($_GET['show']) && $_GET['show'] == 'changelog')
	  echo $forumlink;
    else 
	  echo $currentrev;
 }