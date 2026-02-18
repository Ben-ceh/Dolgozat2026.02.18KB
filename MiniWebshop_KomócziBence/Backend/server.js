const express = require('express');
const app = express();
const mysql = require('mysql2');

const PORT = 3000;
const host = 'localhost'; 

// Beállítjuk a JSON-ben érkező adatok kezelését
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

//kapcsolat beállítása az adatbázissal 
const conn = mysql.createConnection({
    host: 'localhost',
    user: 'root',
    password: '',
    database: 'mini_webshop',
    timezone: 'Z'
})

// Alap route
app.get('/', (req, res) => {
  res.send('Node.js szerver működik!');
});

// összes termék lekérdezése
app.get('/products', (req, res) => {
    conn.query('SELECT * FROM products', (err, result) => {
        if (err) {
            console.log(err);
            return res.status(500).json({ error: 'Hiba a lekérdezés során!' });
        }

        if (result.length === 0) {
            return res.status(404).json({ error: 'Nem található!' });
        }

        return res.status(200).json(result);
    });
});
//Összes kategória
app.get('/category', (req, res) => {
    conn.query('SELECT DISTINCT category FROM products;', (err, result) => {
        if (err) {
            console.log(err);
            return res.status(500).json({ error: 'Hiba a lekérdezés során!' });
        }

        if (result.length === 0) {
            return res.status(404).json({ error: 'Nem található!' });
        }

        return res.status(200).json(result);
    });
});
//új termék
// app.post('/newProduct', (req, res) => {
//     const {name,price,category,inStock} =req.body
//     
//         const sql=`
//                 INSERT INTO 'products' ('id', 'name', 'price', 'category', 'inStock') VALUES (NULL, ?, ?, ?, ?)
//                 `
//         pool.query(sql,[name,price,category,inStock], (err, result) => {
//         if (err) {
//             console.log(err);
//             return res.status(500).json({ error: 'Hiba a lekérdezés során!' });
//         }

//         if (result.length === 0) {
//             return res.status(404).json({ error: 'Nem található!' });
//         }

//         return res.status(200).json(result);
//     });
// });

// app.post('/newProduct', (req, res) => {
//     const {name,price,category,inStock} =req.body
//         const sql=`
//                 INSERT INTO products VALUES (NULL, ?, ?, ?, ?)
//                 `
//         pool.query(sql,[name,price,category,inStock], (err, result) => {
//         if (err) {
//             console.log(err)
//             return res.status(500).json({error:"Hiba"})
//         }
//         if (result.length===0){
//             return res.status(404).json({error:"Nincs adat"})
//         }

//         return res.status(200).json(result)
//         })
// })
app.post('/newProduct',(req,res)=>{
    const {name,price,category,inStock} = req.body;
    if (!name || !price ) {
        return res.status(400).json({error: 'Minden mezőt tölts ki!'})
    }
    connection.query('INSERT INTO products VALUES (NULL, ?, ?, ?, ?)',[name,price,category,inStock],(err,result)=>{
        if(err){
            return res.status(500).send('Adatbázis hiba!')
        }
        console.log(result)
        return res.status(201).json({
            message: 'Sikeres feltöltés'
        })
    })
});

// app.post('/newProduct', (req, res) => {
//         const {name,price,category,inStock} =req.body
//         const sql=`INSERT INTO products VALUES (NULL, ?, ?, ?, ?)`
//         pool.query(sql,[name,price,category,inStock], (err, result) => {
//         if (err) {
//             console.log(err)
//             return res.status(500).json({error:"Hiba"})
//         }
        
//         return res.status(200).json({message:"Sikeres felvitel"})
//         });
// });

//Módosítás
// app.put('/modifyProduct', (req, res) => {
//         const {name,price,category,inStock,id} =req.body
//         const sql=`UPDATE products SET name = ?,price = ?,category = ?, inStock = ? WHERE products.id = ?;`
//         pool.query(sql,[name,price,category,inStock,id], (err, result) => {
//         if (err) {
//             console.log(err)
//             return res.status(500).json({error:"Hiba"})
//         }
        
//         return res.status(200).json({message:"Sikeres felvitel"})
//         });
// });
app.put('/modifyProduct/:id',(req,res)=>{
    const id = Number(req.params.id)
    const {name,price,category,inStock} = req.body;

    if(isNaN(id)||id<=0){
        return res.status(400).json({error: 'Hibás ID!'})
    }

    if (!name || !price || !category || !inStock) {
        return res.status(400).json({error: 'Minden mezőt tölts ki!'})
    }

    const sql = `UPDATE products SET name = ?,price = ?,category = ?, inStock = ? WHERE id = ?`
    connection.query(sql,[name,price,category,inStock,id],(err,result)=>{
        if(err){
            return res.status(500).send('Adatbázis hiba!')
            
        }

        if(result.affectedRows === 0){
            return res.status(404).json({error:"Nincs ilyen ID-jú termék!"})
        }

        return res.status(200).json({message: 'Sikeres módosítás!'}) 
 
    })
});
// delete product 

app.delete('/deleteProduct/:id', (req, res) => {
    const id =Number(req.params.id)
    if(isNaN(id)||id<=0){
        return res.status(400).json({error: 'Hibás ID!'})
    }
        
        const sql=`DELETE from products WHERE id=?`
        connection.query(sql,[id], (err, result) => {
        if (err) {
            console.log(err)
            return res.status(500).json({error:"Nincs ilyen Id-jú termék!"})
        }
        if(result.affectedRows===0){
            return res.status(404).json({error:"Nincs ilyen termék!"})
        }
       console.log(result)
        return res.status(200).json({message:"Sikeres törlés"})
        })
});
//Search
app.post('/searchProduct', (req, res) => {
    const {category,inStock} =req.body
        const sql=` SELECT * FROM products 
                    WHERE category = ? and inStock = ?;
               
                `
        pool.query(sql,[name,price,category,inStock], (err, result) => {
        if (err) {
            console.log(err)
            return res.status(500).json({error:"Hiba"})
        }
        if (result.length===0){
            return res.status(404).json({error:"Nincs adat"})
        }

        return res.status(200).json(result)
        })
})


// Szerver indítása
app.listen(PORT, () => {
    console.log(`Szerver fut: http://${host}:${PORT}`);
  });