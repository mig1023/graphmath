# graph.math

Simple graphic program for python-style algorithm

![](https://s8.hostingkartinok.com/uploads/images/2018/06/543def5021bf6a57954574ed3a6eb7be.png)



### Examples of graphical algorithms:

Ex.:
```
$D = vector( 0, 0 : -3, 4 )
repeat( $A, 1 -> 3 )
	repeat( $B, 1 -> 3 )
		C[$A][$B] = vector( 0, 0 : $A, $B )
		sum( C[$A][$B], $D )
```

![](https://s8.hostingkartinok.com/uploads/images/2018/08/aa854e08ed6d6c819a6677449efefad8.png)

Ex.:
```
repeat( $A, 1 -> 15)
	vector( 0, 0 : -10, $A)
	if ( $A <= 5 )
		vector( 0, 0 : 10, $A )
		vector( 10, $A : 20, 0 )
	vector(-10, $A : -20, 0)
repeat( $B, -5 -> -1 )
	vector( -20, $B : 20, $B )
```

![](https://s8.hostingkartinok.com/uploads/images/2018/08/3e71cdc5281c91bd6ed21990b70185b0.png)

Ex.:
```
$A = 10
repeat( $B, 1 -> 10 )
	$A += 2
	vector( 0, 0 : $B, $A )
	vector( $B, $A : 15, 0 )
```

![](https://s8.hostingkartinok.com/uploads/images/2018/08/5885cc819bb5fca143281f7a60d0f727.png)

Ex.:
```
$A = 10
// $A == 11
if ( $A == 10 )
	vector( 0, 0 : 10, 10 )
	vector( 10, 10 : 20, 0 )
	
	if ( $A == 10)
		vector( 0, 0 : 10, -10 )
		vector( 10, -10 : 20, 0 )
	else
		vector( 0, 0 : -10, -10 )
		vector( -10, -10 : -20 , 0 )
else
	vector( 0, 0 : -10, 10 )
	vector( -10, 10 : -20, 0 )
	
	if ( $A == 10 )
		vector( 0, 0 : 10, -10 )
		vector( 10, -10: 20, 0 )
	else
		vector( 0, 0 : -10, -10 )
		vector( -10,-10 : -20, 0 )
```

![](https://s8.hostingkartinok.com/uploads/images/2018/08/4ae2c48c6b09c08a4aa5845a25d06fdd.png)

Ex.:
```
$B = 20
repeat( $A, 1 -> $B)
	vector( 0, 0 : -10, $A)
	if ( $A <= 5 )
		vector( 0, 0 : 10, $A )
		vector( 10, $A : 20, 0 )
	else
		vector( -20, 0, -30, $A)
		vector( -30, $A, -40, 0) 
	vector(-10, $A : -20, 0)
repeat( $C, -1 -> -5 )
	vector( -20, $C : 20, $C )
```

![](https://s8.hostingkartinok.com/uploads/images/2018/08/045ed3251307aee2064e77a2e982c425.png)
