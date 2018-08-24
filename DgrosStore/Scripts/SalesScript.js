$(document).ready(function () {

    //para efectos en cliente y producto
    $("#searchClient").click(AddInputEffect);
    $("#searchProduct").click(AddInputEffect);

    //clientes y productos
    $("#clientInput").keyup(function () {
        var client = $("#clientInput").val();
        var selectClient = $("#clientSelect");
        $.ajax({
            type: "GET",
            url: "Sales/GetClients",
            data: { client: client },
            success: function (data) {

                data = JSON.parse(data);
                if (data.length === 0 && client.length > 0) {
                    selectClient.html("<option value>No existen clientes con ese nombre</option>");
                } else {
                    selectClient.html("");
                    $(data).each(function (index, item) {
                        selectClient.append(`<option value="${item.ClientId}">${item.Name + " " + item.LastName}</option>`);
                    });
                }

            }
        });

    });
    $("#productInput").keyup(function () {
        var product = $("#productInput").val();
        var selectProduct = $("#productSelect");
        $.ajax({
            type: "GET",
            url: "Sales/GetProducts",
            data: { product: product },
            success: function (data) {

                data = JSON.parse(data);
                if (data.length === 0 && product.length > 0) {
                    selectProduct.html("<option value>No existen productos con ese nombre</option>");
                } else {
                    selectProduct.html("");
                    $(data).each(function (index, item) {
                        selectProduct.append(`<option value="${item.ProductId}">${item.Name}</option>`);
                    });
                }

            }
        });

    });




    //añadir productos
    $("#addProduct").click(function () {
        var selectedProduct = $("#productSelect").val();

        if (selectedProduct > 0) {
            $.ajax({
                type: "GET",
                url: "Sales/GetProduct",
                data: { id: selectedProduct },
                success: function (data) {

                    data = JSON.parse(data);

                    if (!RepeatedProduct(data)) {

                        //append table element
                        CreateTableElement(data);

                        //price
                        UpdatePrice();

                        //productos
                        CreateProductArray();

                        //quantity
                        EventAsingToQuantityAndDiscount(data);


                        //delete button
                        CreateDeleteButtonArray(data);

                    } else {
                        alert("El producto ya esta en la lista de productos");
                    }
                }
            });
        } else {
            alert("Debe elegir un producto");
        }
    });



    //funciones


    //efectos en los input de client y product
    function AddInputEffect() {

        var input = $(this).parent().siblings("input[type='text']");
        var icon = $(this).children("i");

        $(icon).toggleClass("fa fa-remove");
        $(icon).toggleClass("fa fa-search");
        $(input).toggleClass("showIcon");
        $(input).val("");
    }

    //crear elemento de tr de la tabla
    function CreateTableElement(data) {

        $("#tableProduct").append(
            `<tr>
                        <td style="display:none"><input type="hidden" value="${data.id}" readonly></td>
                        <td><strong>${data.name}</strong></td>
                        <td><input type="number" name="quantity" min="1" max="${data.stock}" value="1"></td>
                        <td><input type="number" name="discount" min="0" max="100" value="0"></td>
                        <td><strong name="price">${data.price}</strong></td>
                        <td><strong name="subTotal">${data.price}</strong></td>
                        <td><button type="button" class="btn btn-danger btn-sm"><span class="fa fa-remove"></span></button></td>
                     </tr>`
        );
    }

    //crear Arreglo de ids Productos
    function CreateProductIdArray() {
        var products = Array.from($("#tableProduct")
            .children("tr")
            .children("td")
            .children("input[type='hidden']"));

        return products;
    }

    //ver si se repite el producto
    function RepeatedProduct(data) {

        var products = CreateProductIdArray();
        var productRepeated = false;

        $(products).each(function (index, item) {
            if (parseInt($(item).val()) === data.id) {
                productRepeated = true;
            }
        });
        return productRepeated;
    }


    //actualizar precio
    function UpdatePrice() {

        var total = $("#total");
        var price = 0;

        var ProductPrice = Array
            .from($("#tableProduct")
                .children("tr")
                .children("td")
                .children("strong[name='subTotal']"));

        $(ProductPrice).each(function (i, priceItem) {
            price += Math.round(parseFloat($(priceItem).html()));
        });


        total.html(price);

    }



    //asignar eventos al input cantidades y descuentos
    function EventAsingToQuantityAndDiscount(data) {

        var Productquantity = $("#tableProduct").children("tr").children("td").children("input[name='quantity']");
        var Discountquantity = $("#tableProduct").children("tr").children("td").children("input[name='discount']");
        //var subTotal = $(this).parent().next().next().next().children("strong[name='subTotal']");


        $(Productquantity).last().on("keyup click", data, function () {
            var element = this;
            PriceCal(element, data);
            //price
            UpdatePrice();
        });

        $(Discountquantity).last().on("keyup click", data, function () {
            var element = this;
            PriceCal(element, data);
            //price
            UpdatePrice();
        });


    }


    //calcular precio
    function PriceCal(element, data) {

        var subTotal = $(element).parent().parent().children("td").children("strong[name='subTotal']");
        var discountInput = $(element).parent().parent().children("td").children("input[name='discount']").val();
        var quantityInput = $(element).parent().parent().children("td").children("input[name='quantity']").val();
        var percent;
        var discount;

        if (discountInput.length == 0) {
            alert("El campo de descuento no puede estar vacio");
        } else if (quantityInput.length == 0) {
            alert("El campo de cantidad no puede estar vacio");
        } else {
            percent = parseFloat(parseInt(discountInput) / 100);
            discount = parseFloat(data.price * percent);

            var totalProductPriceWithDiscount = (data.price * quantityInput) - (discount * quantityInput);
            totalProductPriceWithDiscount = Math.round(totalProductPriceWithDiscount);
            $(subTotal).html(totalProductPriceWithDiscount);
        }

    }


    //borrar productos
    function CreateDeleteButtonArray(data) {

        var deleteButton = $("#tableProduct")
            .children("tr")
            .children("td")
            .children("button");

        $(deleteButton).last().on("click", data, function (i, item) {
            if (confirm("Desea borrar este producto?")) {
                $(this).parent().parent().remove();
                UpdatePrice();
            }
        });
    }



    //finalizar venta
    $("#salesButton").click(function () {

        var paymentMethod = parseInt($("#paymentMethod").val());
        var client = $("#clientSelect").val();
        var commentary = $("#commentary").val();
        var total = $("#total").html();
        var products = CreateProductArray(); 
        var notClient = 0;
        var sales;
        console.log();
        if (typeof (products) != "undefined") {

            if (products.length === 0) {
                alert("Debe agregar algun producto para realizar una venta");
            } else {
                if (paymentMethod === 0) {
                    alert("Debe seleccionar un metodo de pago");
                } else {

                    if (client.length == 0) {
                        sales = CreateSalesObject(notClient, products, paymentMethod, commentary, total);
                        if (sales.length != 0) {
                            SendSalesObject(sales);
                        }
                    } else {
                        sales = CreateSalesObject(client, products, paymentMethod, commentary, total);
                        if (sales.length != 0) {
                            SendSalesObject(sales);
                        }
                    }
                }
            }
        }
    });

    //crear arreglo de productos
    function CreateProductArray() {

        var products = $("#tableProduct").children("tr");
        var quantityError = 0;
        var discountError = 0;
        var listOfProducts = [];

        $(products).each(function (i, item) {
            var product = Array.from($(item).children());
            var model = {
                id: parseInt($(product).children("input[type='hidden']").val()),
                quantity: parseInt($(product).children("input[name='quantity']").val()),
                discount: parseInt($(product).children("input[name='discount']").val()),
            }

            if (isNaN(model.quantity)){
                quantityError += 1;
            }
            if (isNaN(model.discount)){
                discountError += 1;
            }
             listOfProducts.push(model);
        });
        if (quantityError > 0) {
            alert("El campo cantidad de algun producto esta vacio, por favor llene el campo");
        } else if (discountError > 0) {
            alert("El campo descuento de algun producto esta vacio, por favor llene el campo");
        } else {
            return listOfProducts;
        }
        
    }



    //crear objeto venta
    function CreateSalesObject(clientId, products, paymentMethod, commentary,total) {
        var Sales = {
            ClientId: clientId,
            Products: products,
            paymentMethod: paymentMethod,
            commentary: commentary,
            Total: total
        }
        return Sales;
    }

    //enviar objeto via ajax
    function SendSalesObject(sales) {

        if (confirm("Desea realizar esta venta")) {
            $.ajax({
                type: "POST",
                url: "Sales/Save",
                dataType: "json",
                data: { model: JSON.stringify(sales) },
                success: function (data) {

                    if (String(data) === "true") {
                        alert("La venta se ha realizado correctamente");
                        window.location.reload();
                    } else {
                        alert("error: " +data);
                    }
                        
                },
                error: function (response) {
                    alert("error: "+response.responseText);
                }
            });
        } else {
            alert("La venta no se ha realizado");
        }
        
    }



});