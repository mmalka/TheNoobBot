<?php
 $currentrev = "1.7.0";
 if (!isset($forwebsite) && !$forwebsite) 
 {
    if (isset($_GET['show']) && $_GET['show'] == 'desc')
	  echo '(WoW 5.3.0.17055)';
    else 
	  echo $currentrev;
 }
 else
 {
   $wow_patch = "5.3.0";
   $forumlink = "http://thenoobbot.com/community/viewtopic.php?f=8&t=6114";
   $month_release = "06";
   $day_release = "12";
   $year_release = "2013";
 }
