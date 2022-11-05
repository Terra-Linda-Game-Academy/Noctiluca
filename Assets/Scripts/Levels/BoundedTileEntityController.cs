namespace Levels {
    public class BoundedTileEntityController<E, C> : TileEntityController<E, C> 
    where E : BoundedTileEntity<E, C>
    where C : TileEntityController<E, C> {
        
    }
}