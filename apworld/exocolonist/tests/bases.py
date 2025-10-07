from test.bases import WorldTestBase

from ..world import ExocolonistWorld
from ..locations import ExocolonistLocation

class ExocolonistTestBase(WorldTestBase):
    game = "I Was a Teenage Exocolonist"
    world: ExocolonistWorld