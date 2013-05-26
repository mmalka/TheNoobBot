<?php
 $currentrev = "1.6.11";
 if (!isset($forwebsite) && !$forwebsite) 
 {
    if (isset($_GET['show']) && $_GET['show'] == 'desc')
	  echo '(WoW 5.3.0.16992)';
    else 
	  echo $currentrev;
 }
 else
 {
   $wow_patch = "5.3.0";
   $forumlink = "http://thenoobbot.com/community/viewtopic.php?f=8&t=6053";
   $month_release = "05";
   $day_release = "24";
   $year_release = "2013";
 }
