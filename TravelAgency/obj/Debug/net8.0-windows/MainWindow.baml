   M S B A M L   `   `   `  ����G  CTravelAgency, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:TravelAgencyTravelAgency  5   	       TravelAgency.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:TravelAgency     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��2* �6   �� 5      $��3* �6   .+�5   
   �� $��0��6   ���#6A0DAD._�5      �� $k��ehttps://www.childinthecity.org/wp-content/uploads/2019/01/architecture-big-city-buildings-1209978.jpg��6   $��UniformToFill=�6�   5      � 5   
   $��1��6   
�������$��Center=�62   $��Center=�6O    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $��* �6    .+�5      �� $%��Добро пожаловать��6   $��26(�5      $
��Bold"�5      ���#6A0DAD �� TextAlignment$ Center=�5       �� 5"      $��1��6   $��
25,0,25,10q�6&   .+�5#      �� $��
Логин��6   ���#6A0DAD6*   �� 5$      -   5         ��Name$ 
fieldLogin��5$      $��30��6.   ���#6A0DAD$��1q�6P   
�������6d   �� 5&      $��Пароль��6   ���#6A0DAD$��0,10,0,0q�6?   G� 5'      -   5         G�Name$ fieldPassword��5'      $��30��65   ���#6A0DAD$��1q�6W   
�������6k   5(      �� 5*      -   5         ��Name$ btnLogin��5*      $��2��6>   $��
Войти��5+      $��40��5,      ���#6A0DAD
�������$
��Bold"�5/      $��0q�50      $��0,10,0,0q�51       � Cursor$
 Hand��52      �� 54      $��3��6   $ ��Забыли пароль?��6%   ���#6A0DAD$��12(�56      $��Right=�57      $��Bottom=�58      $��0,10,0,0q�59      $
 Hand��5:      5;      5<      5=      