/*
    IDA*( state s, int g, threshold t ) {
        h = Eval( s );
        if( h == 0 ) return( true );

        f = g + h;
        if( f > threshold ) return( false );

        for( i = 1; i <= numchildren; i++ ) {
            done = IDA*( s.child[ i ], g + cost( child[ i ] ), t );
            if( done == true ) return( true );
        }
        return( false );
    } 
*/

