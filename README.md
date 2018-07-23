# graph.math

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

Ex.:
```
$A = 10
repeat( $B, 1 -> 10 )
	$A += 2
	vector( 0, 0 : $B, $A )
	vector( $B, $A : 15, 0 )
```

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
