<?php
	$currentrev = "1.8.6.US";
	$wow_patch = "5.4";
	$forumlink = "http://thenoobbot.com/community/viewtopic.php?f=8&t=6308";
	$month_release = "09";
	$day_release = "16";
	$year_release = "2013";
 if (!isset($forwebsite) || !$forwebsite) 
 {
    if (isset($_GET['show']) && $_GET['show'] == 'desc')
	  echo '1.8.6 is for EU/RU/Others - 1.8.6.US is for US';
    elseif (isset($_GET['show']) && $_GET['show'] == 'changelog')
	  echo $forumlink;
    else 
	  echo $currentrev;
 }