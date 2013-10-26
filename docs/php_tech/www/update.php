<?php
	$currentrev = "1.9.5";
	$wow_patch = "5.4";
	$forumlink = "http://thenoobbot.com/community/viewtopic.php?f=8&t=6379";
	$month_release = "10";
	$day_release = "26";
	$year_release = "2013";
 if (!isset($forwebsite) || !$forwebsite) 
 {
    if (isset($_GET['show']) && $_GET['show'] == 'desc')
	  echo '1.9.5 for WoW 5.4.0.17399';
    elseif (isset($_GET['show']) && $_GET['show'] == 'changelog')
	  echo $forumlink;
    else 
	  echo $currentrev;
 }