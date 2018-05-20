// requires nightly version of jQuery b/c it uses the add special event hook
// more about special event hooks: http://brandonaaron.net/blog/2009/06/4/jquery-edge-new-special-event-hooks
(function( $ ) {
	$.event.special.hashchange = {
		setup: function( data, namespaces ) {
			if ( 'onhashchange' in window ) {
				// sweet, native support... short-circuit
				return false;
			}
			// onhashchange is not natively supported ... work around it :(
			fakeHashChange();
		},
		
		add: function( handler, data, namespaces ) {
			return function() {
				arguments[0].hash = getHash();
				handler.apply( this, arguments );
			};
		},
		
		teardown: function( namespaces ) {
			if ( 'onhashchange' in window ) {
				// sweet, native support... short-circuit
				return false;
			}
			// onhashchange is not natively supported ... work around it :(
			unfakeHashChange();
		}
	};
	
	var prevHash = getHash(), interval;
	function fakeHashChange() {
		setInterval( function() {
			var currHash = getHash();
			if ( currHash !== prevHash ) {
				prevHash = currHash;
				$( window ).trigger( 'hashchange' );
			}
		}, 300 );
	}
	
	function unfakeHashChange() {
		clearInterval( interval );
	}
	
	function getHash() {
		var loc = document.location;
		return loc.hash ? loc.href.replace( /^.*\#/, '' ) : '';
	}
})( jQuery );