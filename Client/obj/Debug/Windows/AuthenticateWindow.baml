   M S B A M L   `   `   `  ����A  =Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ClientClient  5   	       Client.AuthenticateWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Client     5   	   $��143.217��5      $��282.86��6+     � 
ResizeMode$  NoResize=�6:    � Title$; 5Аутентификация пользователя��6P   .��5      �  � ColumnDefinitions 5	      �� 5
      �� 5      $��0* �6   .+�5      �� -   5         ��Name$ label��5   
   $��Email��6   $
��Left=�6)   $��	102,5,0,0q�6D   $	��Top=�6W    J� RenderTransformOrigin$ -0.416,-0.372&�6o   $��23��6�   $��36��6�   �� 5      -   5        $ label1��5   
   $��Пароль��6   $
��Left=�6+   $��	97,51,0,0q�6F   $	��Top=�6Y   $��23��6q   $��46��6}   �� 5      -   5         ��Name$ emailControl��5      $
��Left=�6"   $��23��6=   $��	10,28,0,0q�6I    �� TextWrapping$ NoWrap=�6\   $	��Top=�6r   $	��259��6�    �� MaxLines$ 1��6�    �� 	MaxLength$	 128��6�   �� 5      -   5        	 ��Name$	 authenticateButton��5      $"��Аутентификация��6'   $
��Left=�6@   $��	10,98,0,0q�6[   $	��Top=�6n   $	��259��6�   $��20��6�   6�   G� 5      -   5        
 G�Name$
 passwordControl��5      $
��Left=�6)   $��	10,74,0,0q�6D   $	��Top=�6W   $ -3.522,-0.361&�6o   $	��259��6�   $��19��6�   5      5      