   M S B A M L   `   `   `  ����C  ?Paraliso, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:ParalisoParaliso  5   	       Paraliso.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Paraliso     5   	     � Title$  
MainWindow��5   	   $	��700��6   $	��825��6)   $��16(�65    � WindowStartupLocation$ CenterScreen=�6C   .��5	      � $��20q�6    � RowDefinitions 5
   
   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��* �6   .+�5   
   �� -   5        $��txtBlock��5      $��0��6$   $
��10 0q�61   $��28(�6?   �� 6N    � Language$ en-gb�6R   $;�5Image Processing with the power of parallel computing��6c   6�   �� 5   
   -   5         ��Name$ executeSync��5      $��1��6&   $��10q�63   $��10q�6?   $��Execute Serially��6f   �� 5   
   -   5        $ executeParallelAsync��5      $��2��60   $��10q�6=   $��10q�6I   $��Execute in Parallel��6y   �� 5   
   -   5        $ executeAsync��5      $��3��6(   $��10q�65   $��10q�6A   $��Execute Async��6i   � 5   
   -   5         �Name$ dashboardProgress��5       � Value$ 0Q�61   $��30��6;   $��10q�6G   $��4��6S   �� 5   
   $��5��6   $
��Auto=�6$   .��5      �� -   5         ��Name$ resultsWindow��5      $��10q�6/   5      5      5      