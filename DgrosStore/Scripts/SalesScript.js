$(document).ready(function () {

    //para efectos en cliente y producto
    $("#searchClient").click(AddInputEffect);
    $("#searchProduct").click(AddInputEffect);

    //clientes y productos
    
    $("#clientInput").keyup(function () {
        var client = $("#clientInput").val();
        var selectClient = $("#clientSelect");

        if (client.length >= 3 || client.length == 0)
        {  
            $.ajax({
                type: "GET",
                url: "Sales/GetClients",
                data: { param: client },
                success: function (data) {
                    if (data.length === 0 && client.length > 0) {
                        selectClient.html("<option value>No existen registros que coincidan</option>");
                    } else {
                        selectClient.html("");
                        selectClient.append(`<option value="">Seleccione un cliente</option>`);
                        $(data).each(function (index, item) {
                            selectClient.append(`<option value="${item.ClientId}">${item.Name + " " + item.LastName}</option>`);
                        });
                    }
                }
            });
        }
    });

    $("#productInput").keyup(function () {
        var product = $("#productInput").val();
        var selectProduct = $("#productSelect");

        if (product.length >= 3 || product.length == 0) {
            
            $.ajax({
                type: "GET",
                url: "Sales/GetProducts",
                data: { param: product },
                success: function (data) {
                    if (data.length === 0 && product.length > 0) {
                        selectProduct.html("<option value>No existen productos con ese nombre</option>");
                    } else {
                        selectProduct.html("");
                        selectProduct.append(`<option value="">Seleccione un producto</option>`);
                        $(data).each(function (index, item) {
                            selectProduct.append(`<option value="${item.ProductId}">${item.Name}</option>`);
                        });
                    }
                }
            });
        }
    });




    //añadir productos
    $("#addProduct").click(function () {
        var selectedProduct = $("#productSelect").val();
        
        if (selectedProduct > 0) {
            if (!RepeatedProduct(selectedProduct)) {
                $.ajax({
                    type: "GET",
                    url: "Sales/GetProduct",
                    data: { id: selectedProduct },
                    success: function (data) {

                        if (data !== "0") {
                            //append table element
                            CreateTableElement(data);

                            //price
                            UpdatePrice();

                            //productos
                            CreateProductArray();

                            //quantity
                            AsingEventsToQuantityAndDiscount(data);


                            //delete button
                            CreateDeleteButtonArray(data);
                        } else {
                            bootbox.alert("El producto no existe");
                        }
                    }
                });
            } else {
                bootbox.alert("El producto ya se encuentra en la lista de productos");
            }
        } else {
            bootbox.alert("Debe elegir un producto");
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
                        <td style="display:none"><input type="hidden" value="${data.ProductId}" readonly></td>
                        <td><strong name="product">${data.Name}</strong></td>
                        <td><input type="number" name="quantity" min="1" max="${data.Stock}" value="1"></td>
                        <td><input type="number" name="discount" min="0" max="100" value="0"></td>
                        <td><strong name="price">${data.SellingPrice.toFixed(2)}</strong></td>
                        <td><strong name="subTotal">${data.SellingPrice.toFixed(2)}</strong></td>
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
            if (parseInt($(item).val()) === parseInt(data)) {
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
            price += parseFloat($(priceItem).html());
        });
        total.html(price.toFixed(2));
    }



    //asignar eventos al input cantidades y descuentos
    function AsingEventsToQuantityAndDiscount(data) {

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
            discount = parseFloat(data.SellingPrice * percent);

            var totalProductPriceWithDiscount = (data.SellingPrice * quantityInput) - (discount * quantityInput);
            totalProductPriceWithDiscount = totalProductPriceWithDiscount.toFixed(2);
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

            var element = $(this);
            bootbox.confirm("Desea borrar este producto?", function (result) {
                if (result) {
                    element.parent().parent().remove();
                    UpdatePrice();
                }
            });
        });
    }

    function CreateSaleData() {
        
        var sales = {
            Products: CreateProductArray(),
            PaymentMethod: (parseInt($("#paymentMethod").val()) > 0) ? parseInt($("#paymentMethod").val()) : 0,
            DiscountType: (parseInt($("#discountSelect").val()) > 0) ? parseInt($("#discountSelect").val()) : 0,
            Commentary: $("#commentary").val(),
            Total: parseFloat($("#total").html()),
            ClientId: (parseInt($("#clientSelect").val()) > 0) ? parseInt($("#clientSelect").val()) : 0
        }
        return sales;
    }

    //finalizar venta
    $("#salesButton").click(function () {

        var sales = CreateSaleData();

        if (sales.Products.length === 0) {
            bootbox.alert("Debe agregar algun producto para realizar una venta");
        }
        else if (sales.PaymentMethod === 0) {
            bootbox.alert("Debe seleccionar un metodo de pago");
        } else {
            SendSalesObject(sales);
        }
    });

    //crear arreglo de productos
    function CreateProductArray() {

        var products = $("#tableProduct").children("tr");
        var quantityError = 0;
        var discountError = 0;
        var productArray = [];

        $(products).each(function (i, item) {
            var tableTds = Array.from($(item).children());
            var productModel = {
                ProductId: parseInt($(tableTds).children("input[type='hidden']").val()),
                Name:      $(tableTds).children("strong[name='product']").html(),
                Quantity:  parseInt($(tableTds).children("input[name='quantity']").val()),
                Discount:  parseInt($(tableTds).children("input[name='discount']").val()),
                SubTotal:  $(tableTds).children("strong[name='subTotal']").html(),
            }
            if (isNaN(productModel.Quantity)){
                quantityError += 1;
            }
            if (isNaN(productModel.Discount)){
                discountError += 1;
            }
             productArray.push(productModel);
        });
        if (quantityError > 0) {
            bootbox.alert("El campo cantidad de algun producto esta vacio, por favor llene el campo");
        } else if (discountError > 0) {
            bootbox.alert("El campo descuento de algun producto esta vacio, por favor llene el campo");
        } else {
            return productArray;
        }
        
    }


    //enviar objeto via ajax
    function SendSalesObject(sales) {

        bootbox.confirm("Desea realizar esta venta", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "Sales/Save",
                    dataType: "json",
                    data: { data: JSON.stringify(sales) },
                    success: function (data) {

                        if (data === "1") {
                            bootbox.alert("La venta se ha realizado correctamente", function ()
                            {
                                window.location.reload();
                            });
                        } else {
                            bootbox.alert("Error: " + data);
                        }
                    },
                    error: function (response) {
                        bootbox.alert("Error: " + response.responseText);
                    }
                });
            } else {
                bootbox.alert("La venta no se ha realizado");
            }
        });
    }
});