
use billede

CREATE TABLE ImageTable
(
    ImageID INT PRIMARY KEY IDENTITY,
    ImageData VARBINARY(MAX)
);

SELECT COUNT(*) FROM ImageTable;

delete from ImageTable